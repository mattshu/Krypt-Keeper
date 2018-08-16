using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal static class Cipher
    {
        public const int ENCRYPT = 0;
        public const int DECRYPT = 1;
        public const string FILE_EXTENSION = ".krpt";
        public const string WORKING_FILE_EXTENSION = ".krpt.tmp";
        private const int MINIMUM_FILE_LENGTH = 16; // IV
        private const int CHUNK_SIZE = 16 * 1024 * 1024; // 16MB
        public static bool CancelProcessing = false;
        private static BackgroundWorker _backgroundWorker;
        private static readonly Status _status = Status.GetInstance();
        private static DateTime _cipherStartTime;

        public static void ProcessFiles(CipherOptions options)
        {
            if (options.Files.Length <= 0) return;
            _cipherStartTime = DateTime.Now;
            _backgroundWorker.RunWorkerAsync(options);
        }

        public static void SetBackgroundWorker(BackgroundWorker bgWorker)
        {
            _backgroundWorker = bgWorker;
            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = (CipherOptions) e.Argument;
            for (int i = 0; i < options.Files.Length; i++)
            {
                var path = options.Files[i];
                _status.UpdateProgressTotal((int) Math.Round((double) (100 * i) / options.Files.Length));
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

        private static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (MainWindow.CloseAfterCurrentOperation) return;
            _status.UpdateProgressCurrent(e.ProgressPercentage);
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (MainWindow.CloseAfterCurrentOperation) return;
            _status.WriteLine("Operation finished. " + Helper.GetSpannedTime(_cipherStartTime.Ticks));
            _status.UpdateProgressCurrent(0);
            _status.UpdateProgressTotal(0);
        }

        private static void process(string path, CipherOptions options)
        {
            var workingPath = "";
            try
            {
                if (!File.Exists(path))
                {
                    _status.WriteLine("* File not found: " + path);
                    return;
                }
                if (options.Mode == DECRYPT && (Path.GetExtension(path) != FILE_EXTENSION ||
                                                new FileInfo(path).Length <= MINIMUM_FILE_LENGTH))
                {
                    _status.WriteLine("* File not in correct format for decryption: " + path);
                    return;
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
                    aes.Key = options.Key;
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
                                wStream.Write(options.IV, 0, options.IV.Length); // Insert IV
                            using (var cStream = new CryptoStream(wStream, transformer, CryptoStreamMode.Write))
                            {
                                rStream.Seek(options.Mode == ENCRYPT ? 0 : 16,
                                    SeekOrigin.Begin); // 16 skips IV if decrypting
                                processStreams(path, rStream, cStream);
                                if (options.Mode == ENCRYPT)
                                    buildAndWriteFooter(path, cStream);
                            }
                        }
                    }
                }
                if (CancelProcessing)
                {
                    _status.WriteLine("Process cancelled, deleting temp file: " + workingPath);
                    Helper.SafeFileDelete(workingPath);
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
                    Helper.SafeFileDelete(workingPath); // Remove temp file
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
                Helper.SafeFileDelete(workingPath); // Remove temp file
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

        private static void buildAndWriteFooter(string path, Stream cryptoStream)
        {
            var footer = new Footer();
            footer.Build(path);
            var footerData = footer.ToArray();
            cryptoStream.Write(footerData, 0, footerData.Length);
        }

        private static void processStreams(string path, Stream readStream, Stream cryptoStream)
        {
            CancelProcessing = false;
            var buffer = new byte[CHUNK_SIZE];
            int bytesRead = readStream.Read(buffer, 0, CHUNK_SIZE);
            var totalBytes = new FileInfo(path).Length;
            long progressBytes = 0;
            while (bytesRead > 0)
            {
                if (CancelProcessing)
                    break;
                progressBytes += bytesRead;
                _backgroundWorker.ReportProgress((int) Math.Round((double) (100 * progressBytes) / totalBytes));
                cryptoStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, bytesRead);
            }
        }

        private static void postProcessFileHandling(string path, string workingPath, CipherOptions options)
        {
            if (options.Mode == ENCRYPT)
            {
                if (options.RemoveOriginal)
                    Helper.SafeFileDelete(path);
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
                    Helper.SafeFileDelete(path); // Delete original encrypted file
                    Helper.SafeFileDelete(workingPath); // Delete temp file
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