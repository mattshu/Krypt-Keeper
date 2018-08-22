using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KryptKeeper
{
    internal static class Cipher
    {
        public const int ENCRYPT = 0;
        public const int DECRYPT = 1;
        public const string FILE_EXTENSION = ".krpt";
        public const string WORKING_FILE_EXTENSION = ".krpt.tmp";
        public const long MAX_FILE_LENGTH = 0x800000000; // 4GB
        private const int MINIMUM_FILE_LENGTH = IV_SIZE + SALT_SIZE;
        private const int IV_SIZE = 16;
        private const int SALT_SIZE = 29;
        private const int KEY_SIZE = 256; 
        private const int CHUNK_SIZE = 0x1000000; // 16MB
        public static void CancelProcessing() => _cancelProcessing = true;
        private static bool _cancelProcessing;
        private static BackgroundWorker _backgroundWorker;
        private static readonly Status _status = Status.GetInstance();
        public static string GetElapsedTime() => Helper.GetSpannedTime(_cipherStartTime.Ticks);
        private static DateTime _cipherStartTime;
        private static long _progressBytesOverall;
        private static long _progressBytesTotal;

        public static void ProcessFiles(CipherOptions options)
        {
            if (options.Files.Length <= 0) return;
            _progressBytesOverall = 0;
            _progressBytesTotal = options.CalculateTotalPayload();
            _cipherStartTime = DateTime.Now;
            _backgroundWorker.RunWorkerAsync(options);
        }

        public static void SetBackgroundWorker(BackgroundWorker bgWorker)
        {
            _backgroundWorker = bgWorker;
            _backgroundWorker.DoWork += backgroundWorker_DoWork;
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = (CipherOptions) e.Argument;
            foreach (var path in options.Files)
            {
                if (_backgroundWorker.CancellationPending)
                    break;
                if (File.Exists(path))
                {
                    _status.WritePending($"{options.GetModeOfOperation()}: " + path);
                    process(path, options);
                }
                else
                    _status.WriteLine("* Unable to find: " + path);
            }
        }

        private static void process(string path, CipherOptions options)
        {
            string workingPath = "";
            try
            {
                if (!File.Exists(path))
                {
                    _status.WriteLine("* File not found: " + path);
                    return;
                }
                if (options.Mode == DECRYPT && !isFileValidForDecryption(path)) return;
                workingPath = options.Mode == ENCRYPT ? path + FILE_EXTENSION : path.ReplaceLastOccurrence(FILE_EXTENSION, WORKING_FILE_EXTENSION);
                if (File.Exists(workingPath)) workingPath = Helper.PadExistingFileName(workingPath);
                using (var aes = Aes.Create())
                {
                    if (aes == null)
                    {
                        _status.WriteLine("*** Error: Failed to create AES object!");
                        return;
                    }
                    var transformer = createCryptoTransform(path, options, aes);
                    using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (var wStream = new FileStream(workingPath, FileMode.CreateNew, FileAccess.Write))
                        {
                            if (options.Mode == ENCRYPT)
                            {
                                wStream.Write(options.IV, 0, options.IV.Length); // Insert IV
                                wStream.Write(options.Salt, 0, options.Salt.Length); // Insert Salt
                            }
                            using (var cStream = new CryptoStream(wStream, transformer, CryptoStreamMode.Write))
                            {
                                // 45 skips IV (16 bytes) and salt (29 bytes) if decrypting
                                rStream.Seek(options.Mode == ENCRYPT ? 0 : 45, SeekOrigin.Begin); 
                                processStreams(path, rStream, cStream);
                                if (options.Mode == ENCRYPT)
                                    buildAndWriteFooter(path, cStream);
                            }
                        }
                    }
                }
                if (_cancelProcessing)
                {
                    _status.WriteLine("Process cancelled, deleting temp file: " + workingPath);
                    if (!Helper.TryDeleteFile(workingPath))
                        _status.WriteLine("Unable to remove temp file: " + workingPath);
                }
                else
                    postProcessFileHandling(path, workingPath, options);
            }
            catch (Exception ex)
            {
                handleCipherExceptions(ex, path, workingPath, options);
            }
        }

        private static bool isFileValidForDecryption(string path)
        {
            if (Path.GetExtension(path) == FILE_EXTENSION && new FileInfo(path).Length >= MINIMUM_FILE_LENGTH)
                return true;
            _status.WriteLine("* File not in correct format for decryption: " + path);
            return false;

        }

        private static ICryptoTransform createCryptoTransform(string path, CipherOptions options, SymmetricAlgorithm aes)
        {
            aes.KeySize = KEY_SIZE;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform transformer;
            if (options.Mode == ENCRYPT)
            {
                aes.Key = GenerateSaltedKey(options.Key, options.Salt);
                aes.IV = options.IV;
                transformer = aes.CreateEncryptor(aes.Key, aes.IV);
            }
            else
            {
                aes.Key = GenerateSaltedKey(options.Key, extractSalt(path));
                aes.IV = extractIV(path);
                transformer = aes.CreateDecryptor(aes.Key, aes.IV);
            }
            return transformer;
        }

        public static byte[] GenerateSaltedKey(byte[] key, byte[] salt)
        { // for Decryption
            var saltedKey = BCrypt.HashPassword(Encoding.UTF8.GetString(key), Encoding.UTF8.GetString(salt));
            return Helper.GetSHA256(Helper.GetBytes(saltedKey));
        }

        private static byte[] extractIV(string path)
        {
            try
            {
                using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var bReader = new BinaryReader(fStream))
                    {
                        var read = bReader.ReadBytes(16);
                        if (read.Length > 0)
                            return read;
                        throw new CryptographicException("There was a problem extracting the IV from: " + path);
                    }
                }
            }
            catch (Exception ex)
            {
                _status.WriteLine("There was a problem extracting the IV from: " + path);
                _status.WriteLine("* Error: " + ex.Message);
                return new byte[0];
            }
        }

        private static byte[] extractSalt(string path)
        {
            try
            {
                using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var bReader = new BinaryReader(fStream))
                    {
                        fStream.Seek(16, SeekOrigin.Begin);
                        var read = bReader.ReadBytes(29);
                        if (read.Length > 0)
                            return read;
                        throw new CryptographicException("There was a problem extracting the salt from: " + path);
                    }
                }
            }
            catch (Exception ex)
            {
                _status.WriteLine("There was a problem extracting the salt from: " + path);
                _status.WriteLine("* Error: " + ex.Message);
                return new byte[0];
            }
        }

        private static void buildAndWriteFooter(string path, Stream cryptoStream)
        {
            var footer = new Footer();
            footer.Build(path);
            var footerData = footer.ToArray();
            cryptoStream.Write(footerData, 0, footerData.Length);
        }

        private static void processStreams(string path, Stream readStream, Stream cryptoStream)
        {
            _cancelProcessing = false;
            var buffer = new byte[CHUNK_SIZE];
            int bytesRead = readStream.Read(buffer, 0, CHUNK_SIZE);
            var totalBytes = new FileInfo(path).Length;
            long progressBytes = 0;
            while (bytesRead > 0)
            {
                if (_cancelProcessing)
                    break;
                progressBytes += bytesRead;
                _progressBytesOverall += bytesRead;
                _backgroundWorker.ReportProgress(Helper.GetPercentProgress(progressBytes, totalBytes), Helper.GetPercentProgress(_progressBytesOverall, _progressBytesTotal));
                cryptoStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, bytesRead);
            }
        }

        private static void postProcessFileHandling(string path, string workingPath, CipherOptions options)
        {
            if (options.Mode == ENCRYPT)
                encryptionPostProcess(path, workingPath, options);
            else
                decryptionPostProcess(path, workingPath);
        }

        private static void encryptionPostProcess(string path, string workingPath, CipherOptions options)
        {
            if (options.RemoveOriginal && !Helper.TryDeleteFile(path))
                _status.WriteLine("Unable to remove original (access denied): " + path);
            if (options.MaskFileName)
                File.Move(workingPath,
                    workingPath = workingPath.Replace(Path.GetFileName(workingPath).RemoveDefaultFileExt(),
                        Helper.GetRandomAlphanumericString(16)));
            if (options.MaskFileTimes)
                Helper.SetRandomFileTimes(workingPath);
        }

        private static void decryptionPostProcess(string path, string workingPath)
        {
            var footer = new Footer();
            if (!footer.TryExtract(workingPath))
            {
                _status.WriteLine("*** Error: Unable to get decrypted file information from " + path);
                _status.WriteLine("* File possibly corrupt: " + workingPath);
                return;
            }
            using (var fOpen = new FileStream(workingPath, FileMode.Open))
                fOpen.SetLength(fOpen.Length - footer.ToArray().Length);
            var originalPath = workingPath.Replace(Path.GetFileName(workingPath), footer.Name)
                .Replace(WORKING_FILE_EXTENSION, "");
            if (File.Exists(originalPath))
                originalPath = Helper.PadExistingFileName(originalPath);
            File.Move(workingPath, originalPath); // Copy worked file to original file
            Helper.SetFileTimesFromFooter(originalPath, footer);
            if (new FileInfo(originalPath).Length > 0)
            {
                if (!Helper.TryDeleteFile(path))
                    _status.WriteLine("Unable to remove original file: " + path);
            }
            else
            {
                _status.WriteLine("* Error decrypting file: " + path);
                _status.WriteLine("* File possibly corrupt: " + originalPath);
            }
            if (!Helper.TryDeleteFile(workingPath))
                _status.WriteLine("Unable to remove temp file: " + workingPath);
        }

        private static void handleCipherExceptions(Exception ex, string path, string workingPath, CipherOptions options)
        {
            var baseExceptionRaw = ex.GetBaseException().ToString();
            var baseExceptionPart = baseExceptionRaw.Substring(0, baseExceptionRaw.IndexOf(':'));
            var baseException = baseExceptionPart.Substring(baseExceptionPart.LastIndexOf('.') + 1);
            var preserveTempFile = false;
            switch (baseException)
            {
                case "CryptographicException":
                    if (ex.Message.Equals("The input data is not a complete block.") ||
                        ex.Message.Equals("Padding is invalid and cannot be removed."))
                        _status.WriteLine("*** Failed to decrypt file: " + path);
                    else
                        _status.WriteLine("*** Cryptographic exception: " + ex.Message + Environment.NewLine +
                                          ex.StackTrace);
                    break;
                case "ArgumentException":
                    _status.WriteLine("*** Failed to decrypt file: " + path);
                    break;
                case "UnauthorizedAccessException":
                    _status.WriteLine("*** Unauthorized access: " + ex.Message);
                    break;
                default:
                    _status.WriteLine("*** UNHANDLED EXCEPTION: " + ex.Message + Environment.NewLine + ex.StackTrace);
                    _status.WriteLine("Partially processed file will be preserved: " + workingPath);
                    preserveTempFile = true;
                    break;
            }
            if (!preserveTempFile && !Helper.TryDeleteFile(workingPath))
                _status.WriteLine("Unable to remove temp file: " + workingPath);
        }
    }
}