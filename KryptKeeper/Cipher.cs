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
        private const int MINIMUM_FILE_LENGTH = IV_SIZE + SALT_SIZE + SECURE_RANDOM_FILLER_SIZE;
        private const int IV_SIZE = 16;
        private const int SALT_SIZE = 29;
        private const int SECURE_RANDOM_FILLER_SIZE = 15;
        private const int KEY_SIZE = 256;
        private const int CHUNK_SIZE = 0x1000000; // 16MB

        private static bool _cancelProcessing;
        private static BackgroundWorker _backgroundWorker;
        private static readonly Status _status = Status.GetInstance();

        private static DateTime _cipherStartTime;
        private static long _progressBytesOverall;
        private static long _progressBytesTotal;
        private static int _progressFileIndex = 0;
        private static int _progressFileTotal;

        public static void CancelProcessing() => _cancelProcessing = true;
        public static string GetElapsedTime(bool hideMs = false) => Helper.GetSpannedTime(_cipherStartTime.Ticks, hideMs);

        public static string GetFileProgress() => $"{_progressFileIndex}/{_progressFileTotal} files processed";

        public static void ProcessFiles(CipherOptions options)
        {
            if (options.Files.Count <= 0) return;
            _progressBytesOverall = 0;
            _progressFileIndex = 0;
            _progressFileTotal = options.Files.Count;
            _progressBytesTotal = Helper.CalculateTotalFilePayload(options.Files);
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
            var options = (CipherOptions)e.Argument;
            foreach (var fileData in options.Files.GetList())
            {
                if (_backgroundWorker.CancellationPending)
                    break;
                if (File.Exists(fileData.GetFilePath()))
                {
                    _status.WritePending($"{options.GetModeOfOperation()}: " + fileData.GetFilePath());
                    _status.UpdateOperationLabel(options.GetModeOfOperation());
                    processFile(fileData.GetFilePath(), options);
                    _progressFileIndex++;
                }
                else
                    _status.WriteLine("* Unable to find: " + fileData);
            }
        }

        private static void processFile(string path, CipherOptions options)
        {
            string workingPath = "";
            try
            {
                if (!File.Exists(path))
                {
                    _status.WriteLine("* File not found: " + path);
                    return;
                }
                _status.UpdateProcessingLabel(Path.GetFileName(path));
                if (options.Mode == DECRYPT && !isFileValidForDecryption(path)) return;
                workingPath = options.Mode == ENCRYPT
                    ? path + FILE_EXTENSION
                    : path.ReplaceLastOccurrence(FILE_EXTENSION, WORKING_FILE_EXTENSION);
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
                                injectHeader(options, wStream);
                            using (var cStream = new CryptoStream(wStream, transformer, CryptoStreamMode.Write))
                            {
                                rStream.Seek(options.Mode == ENCRYPT ? 0 : SECURE_RANDOM_FILLER_SIZE + IV_SIZE + SALT_SIZE, SeekOrigin.Begin);
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
                {
                    var resultFile = postProcessFileHandling(path, workingPath, options);
                    if (resultFile.Length > 0)
                        _status.WriteLine("Processed to: " + Path.GetFileName(resultFile));
                }
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
                aes.Key = generateSaltedKey(options.Key, options.Salt);
                aes.IV = options.IV;
                transformer = aes.CreateEncryptor(aes.Key, aes.IV);
            }
            else
            {
                aes.Key = generateSaltedKey(options.Key, extractSalt(path));
                aes.IV = extractIV(path);
                transformer = aes.CreateDecryptor(aes.Key, aes.IV);
            }
            return transformer;
        }

        private static byte[] generateSaltedKey(byte[] key, byte[] salt)
        {
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
                        fStream.Seek(SECURE_RANDOM_FILLER_SIZE, SeekOrigin.Begin);
                        var read = bReader.ReadBytes(IV_SIZE);
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
                        fStream.Seek(IV_SIZE + SECURE_RANDOM_FILLER_SIZE, SeekOrigin.Begin);
                        var read = bReader.ReadBytes(SALT_SIZE);
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

        private static void injectHeader(CipherOptions options, Stream stream)
        {
            stream.Write(options.SecureRandomFiller, 0, options.SecureRandomFiller.Length); // Insert random filler bytes
            stream.Write(options.IV, 0, options.IV.Length); // Insert IV
            stream.Write(options.Salt, 0, options.Salt.Length); // Insert Salt
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

        private static void buildAndWriteFooter(string path, Stream cryptoStream)
        {
            var footer = new Footer();
            footer.Build(path);
            var footerData = footer.ToArray();
            cryptoStream.Write(footerData, 0, footerData.Length);
        }

        private static string postProcessFileHandling(string path, string workingPath, CipherOptions options)
        {
            return options.Mode == ENCRYPT ? encryptionPostProcess(path, workingPath, options) : decryptionPostProcess(path, workingPath, options);
        }

        private static string encryptionPostProcess(string path, string workingPath, CipherOptions options)
        {
            if (options.RemoveOriginalEncryption && !Helper.TryDeleteFile(path))
                _status.WriteLine("Unable to remove original (access denied): " + path);
            if (options.MaskFileName)
            {
                var originalWorkingPath = workingPath;
                workingPath = workingPath.Replace(Path.GetFileName(workingPath).RemoveDefaultFileExt(), Helper.GetRandomAlphanumericString(16));
                File.Move(originalWorkingPath, workingPath);
            }
            if (options.MaskFileDate)
                Helper.SetRandomFileTimes(workingPath);
            return workingPath;
        }

        private static string decryptionPostProcess(string path, string workingPath, CipherOptions options)
        {
            var footer = new Footer();
            if (!footer.TryExtract(workingPath))
            {
                _status.WriteLine("*** Error: Unable to get decrypted file information from " + path);
                _status.WriteLine("* File possibly corrupt: " + workingPath);
                return string.Empty;
            }
            using (var fOpen = new FileStream(workingPath, FileMode.Open))
                fOpen.SetLength(fOpen.Length - footer.ToArray().Length);
            var originalPath = workingPath.Replace(Path.GetFileName(workingPath), footer.Name)
                .ReplaceLastOccurrence(WORKING_FILE_EXTENSION, "");
            if (File.Exists(originalPath))
                originalPath = Helper.PadExistingFileName(originalPath);
            File.Move(workingPath, originalPath); // Copy worked file to original file
            Helper.SetFileTimesFromFooter(originalPath, footer);
            if (new FileInfo(originalPath).Length > 0)
            {
                if (options.RemoveOriginalDecryption && !Helper.TryDeleteFile(path))
                    _status.WriteLine("Unable to remove original file: " + path);
            }
            else
            {
                _status.WriteLine("* Error decrypting file: " + path);
                _status.WriteLine("* File possibly corrupt: " + originalPath);
                return string.Empty;
            }
            if (!Helper.TryDeleteFile(workingPath))
                _status.WriteLine("Unable to remove temp file: " + workingPath);
            return originalPath;
        }

        private static void handleCipherExceptions(Exception ex, string path, string workingPath, CipherOptions options)
        {
            var baseExceptionRaw = ex.GetBaseException().ToString();
            var baseExceptionPart = baseExceptionRaw.Substring(0, baseExceptionRaw.IndexOf(':'));
            var baseException = baseExceptionPart.Substring(baseExceptionPart.LastIndexOf('.') + 1);
            var msgWhenUnhandled = ex.Message + Environment.NewLine + ex.StackTrace;
            var preserveTempFile = false;
            switch (baseException)
            {
                case "CryptographicException":
                    if (ex.Message.Equals("The input data is not a complete block.") ||
                        ex.Message.Equals("Padding is invalid and cannot be removed."))
                        _status.WriteLine("*** Failed to decrypt file: " + path);
                    else
                        _status.WriteLine("*** Cryptographic exception: " + msgWhenUnhandled);
                    break;

                case "ArgumentException":
                    _status.WriteLine("*** Failed to decrypt file: " + path);
                    break;

                case "UnauthorizedAccessException":
                    _status.WriteLine("*** Unauthorized access: " + ex.Message);
                    break;

                default:
                    _status.WriteLine("*** UNHANDLED EXCEPTION: " + msgWhenUnhandled);
                    _status.WriteLine("Partially processed file will be preserved: " + workingPath);
                    preserveTempFile = true;
                    break;
            }
            if (!preserveTempFile && !Helper.TryDeleteFile(workingPath))
                _status.WriteLine("Unable to remove temp file: " + workingPath);
        }
    }
}