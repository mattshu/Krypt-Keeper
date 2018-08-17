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
                if (options.Mode == DECRYPT)
                {
                    if (Path.GetExtension(path) != FILE_EXTENSION || new FileInfo(path).Length <= MINIMUM_FILE_LENGTH)
                    {
                        _status.WriteLine("* File not in correct format for decryption: " + path);
                        return;
                    }
                }
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
                    var key = new byte[32];
                    if (options.Mode == ENCRYPT)
                        key = Helper.GenerateSaltedKey(options.Key, options.Salt);
                    if (options.Mode == DECRYPT)
                    {
                        var saltString = extractSaltString(path);
                        if (saltString == null)
                        {
                            _status.WriteLine("*** Error: Unable to extract salt from: " + path);
                            return;
                        }
                        key = Helper.GenerateSaltedKey(options.Key, Helper.GetBytes(saltString));
                    }
                    aes.KeySize = KEY_SIZE;
                    aes.Key = key;
                    aes.IV = options.Mode == ENCRYPT ? options.IV : extractIV(path);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    var transformer = options.Mode == ENCRYPT
                        ? aes.CreateEncryptor(aes.Key, aes.IV)
                        : aes.CreateDecryptor(aes.Key, aes.IV);
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
                    return;
                }
                postProcessFileHandling(path, workingPath, options);
            }
            catch (CryptographicException ex)
            {
                if (ex.Message.Equals("The input data is not a complete block.",
                    StringComparison.InvariantCultureIgnoreCase) || ex.Message.Equals("Padding is invalid and cannot be removed.",
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    _status.WriteLine("*** Failed to decrypt file: " + path);
                    if (!Helper.TryDeleteFile(workingPath))
                        _status.WriteLine("Unable to remove temp file: " + workingPath);
                }
                else
                {
                    _status.WriteLine("*** Cryptographic exception: " + ex.Message);
                    _status.WriteLine("Stacktrace: " + ex.StackTrace);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _status.WriteLine("*** Unauthorized access: " + ex.Message);
                if (!Helper.TryDeleteFile(workingPath))
                    _status.WriteLine("Unable to remove temp file: " + workingPath);
            }
            catch (Exception ex)
            {
                _status.WriteLine("*** UNHANDLED EXCEPTION: " + ex.Message);
                _status.WriteLine("* STACKTRACE: " + ex.StackTrace);
                _status.WriteLine("Preserving " + workingPath + " for debugging purposes."); // Keep temp file
            }
        }

        private static byte[] extractIV(string path)
        {
            using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var bReader = new BinaryReader(fStream))
                {
                    return bReader.ReadBytes(16);
                }
            }
        }

        private static string extractSaltString(string path)
        {
            using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var bReader = new BinaryReader(fStream))
                {
                    fStream.Seek(16, SeekOrigin.Begin);
                    return Encoding.UTF8.GetString(bReader.ReadBytes(29));
                }
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
            else
            {
                var footer = new Footer();
                if (!footer.TryExtract(workingPath))
                {
                    _status.WriteLine("*** Error: Unable to generate decrypted file information from " + path);
                    _status.WriteLine("* File possibly corrupt: " + workingPath);
                    return;
                }
                using (var fOpen = new FileStream(workingPath, FileMode.Open))
                {
                    fOpen.SetLength(fOpen.Length - footer.ToArray().Length);
                }
                var originalPath = workingPath.Replace(Path.GetFileName(workingPath), footer.Name)
                    .Replace(WORKING_FILE_EXTENSION, "");
                if (File.Exists(originalPath))
                    originalPath = Helper.PadExistingFileName(originalPath);
                File.Move(workingPath, originalPath); // Copy temp file to original file
                Helper.SetFileTimesFromFooter(originalPath, footer);
                if (new FileInfo(originalPath).Length > 0)
                {
                    if (!Helper.TryDeleteFile(path))
                        _status.WriteLine("Unable to remove original file: " + path);
                    if (!Helper.TryDeleteFile(workingPath))
                        _status.WriteLine("Unable to remove temp file: " + workingPath);
                }
                else
                {
                    _status.WriteLine("* Error decrypting file: " + path);
                    _status.WriteLine("* File possibly corrupt: " + originalPath);
                }
            }
        }
    }
}