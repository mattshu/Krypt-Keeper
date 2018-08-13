using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal static class Cipher
    {

        public static bool CancelProcessing = false;
        public const string FILE_EXTENSION = ".krpt";
        public const string WORKING_FILE_EXTENSION = ".krpt.tmp";
        private const int MINIMUM_FILE_LENGTH = 16; // IV
        private static BackgroundWorker _backgroundWorker;
        private const int CHUNK_SIZE = 16 * 1024 * 1024; // 16MB
        private static readonly Status _status = Status.GetInstance();
        private static DateTime cipherStartTime;

        public static void ProcessFiles(CipherOptions options)
        {
            if (options.Files.Length <= 0) return;
            cipherStartTime = DateTime.Now;
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
            try
            {
                var options = (CipherOptions)e.Argument;
                foreach (var path in options.Files)
                {
                    if (_backgroundWorker.CancellationPending)
                        break;
                    if (!File.Exists(path))
                    {
                        _status.WriteLine("* Unable to find: " + path);
                        continue;
                    }
                    _status.WritePending($"{options.GetCipherModeOfOperation()}: " + path);
                    if (options.Mode == CipherOptions.ENCRYPT)
                        encrypt(path, options);
                    else
                        decrypt(path, options);
                }
            }
            catch (CryptographicException ex)
            {
                _status.WriteLine($"*** Unable to decrypt {ex.Message}. Check your key or the file format.");
            }
            catch (UnauthorizedAccessException ex)
            {
                _status.WriteLine($"*** Unable to access {ex.Message}");
            }
            catch (Exception ex)
            {
                _status.WriteLine($"*** Unhandled exception occured: ({ex.Message})" + Environment.NewLine + ex.StackTrace);
            }
        }

        private static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (MainWindow.CloseAfterCurrentOperation) return;
            _status.UpdateProgress(e.ProgressPercentage);
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (MainWindow.CloseAfterCurrentOperation) return;
            _status.WriteLine("Operation finished. " + Helper.GetSpannedTime(cipherStartTime.Ticks));
            _status.UpdateProgress(0);
        }

        private static void decrypt(string path, CipherOptions options)
        {
            var decryptedPath = "";
            try
            {
                if (!File.Exists(path) || Path.GetExtension(path) != FILE_EXTENSION || new FileInfo(path).Length <= MINIMUM_FILE_LENGTH)
                {
                    _status.WriteLine("* File not found or not in correct format for decryption: " + path);
                    return;
                }
                using (var aes = Aes.Create())
                {
                    if (aes == null)
                    {
                        _status.WriteLine("*** Error: Failed to create AES object!");
                        return;
                    }
                    aes.Key = options.Key;
                    aes.IV = extractIV(path);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    decryptedPath = path.ReplaceLastOccurrence(FILE_EXTENSION, WORKING_FILE_EXTENSION);
                    if (File.Exists(decryptedPath)) decryptedPath = Helper.PadExistingFileName(decryptedPath);
                    using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (var wStream = new FileStream(decryptedPath, FileMode.CreateNew, FileAccess.Write))
                        {
                            using (var cStream = new CryptoStream(wStream, decryptor, CryptoStreamMode.Write))
                            {
                                initializeDecryption(path, rStream, cStream);
                            }
                        }
                    }
                    if (_backgroundWorker.CancellationPending)
                    {
                        if (!CancelProcessing)
                            _status.WriteLine("Deleting temp file: " + decryptedPath);
                        Helper.SafeFileDelete(decryptedPath);
                        CancelProcessing = false;
                        return;
                    }
                    if (File.Exists(decryptedPath) && new FileInfo(decryptedPath).Length > 0)
                    {
                        Helper.SafeFileDelete(path);
                        postDecryptionFileHandling(decryptedPath);
                        return;
                    }
                    _status.WriteLine("*** Error: Failed to decrypt file: " + path);
                    Helper.SafeFileDelete(decryptedPath);
                }
            }
            catch (CryptographicException)
            {
                Helper.SafeFileDelete(decryptedPath);
                throw new CryptographicException(path);
            }
            catch (UnauthorizedAccessException)
            {
                Helper.SafeFileDelete(decryptedPath);
                throw new UnauthorizedAccessException(path);
            }
        }

        private static void encrypt(string path, CipherOptions options)
        {
            try
            {
                if (!File.Exists(path))
                {
                    _status.WriteLine("* File not found: " + path);
                    return;
                }
                var pathWithExt = path + FILE_EXTENSION;
                if (File.Exists(pathWithExt)) pathWithExt = Helper.PadExistingFileName(pathWithExt);
                using (var aes = Aes.Create())
                {
                    if (aes == null)
                    {
                        _status.WriteLine("*** Error: Failed to create AES object!");
                        return;
                    }
                    aes.Key = options.Key;
                    aes.IV = options.IV;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (var wStream = new FileStream(pathWithExt, FileMode.CreateNew, FileAccess.Write))
                        {
                            wStream.Write(options.IV, 0, options.IV.Length);
                            using (var cStream = new CryptoStream(wStream, encryptor, CryptoStreamMode.Write))
                            {
                                initializeEncryption(path, rStream, cStream);
                            }
                        }
                    }
                }

                if (_backgroundWorker.CancellationPending)
                {
                    Helper.SafeFileDelete(pathWithExt);
                    if (!CancelProcessing)
                        _status.WriteLine("Deleting temp file: " + pathWithExt);
                    else
                        return;
                }
                postEncryptionFileHandling(pathWithExt, options);
            }
            catch (CryptographicException)
            {
                throw new CryptographicException(path);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException(path);
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

        private static void initializeDecryption(string path, Stream readStream, Stream cryptoStream)
        {
            readStream.Seek(16, SeekOrigin.Begin); // 16 to skip IV
            processStreams(path, readStream, cryptoStream);
        }

        private static void initializeEncryption(string path, Stream readStream, Stream cryptoStream)
        {
            readStream.Seek(0, SeekOrigin.Begin);
            processStreams(path, readStream, cryptoStream);
            if (!_backgroundWorker.CancellationPending)
                buildAndWriteFooter(path, cryptoStream);
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

        private static void postDecryptionFileHandling(string decryptedPath)
        {
            var footer = new Footer();
            if (!footer.TryExtract(decryptedPath))
            {
                Helper.SafeFileDelete(decryptedPath);
                _status.WriteLine("* Error: Unable to generate file information from " + decryptedPath);
                return;
            }
            using (var fOpen = new FileStream(decryptedPath, FileMode.Open))
            {
                fOpen.SetLength(fOpen.Length - footer.ToArray().Length);
            }
            var projectedPath = decryptedPath.Replace(Path.GetFileName(decryptedPath), footer.Name).Replace(WORKING_FILE_EXTENSION, "");
            if (File.Exists(projectedPath))
                projectedPath = Helper.PadExistingFileName(projectedPath);
            File.Move(decryptedPath, projectedPath);
            Helper.SetFileTimesFromFooter(projectedPath, footer);
        }

        private static void postEncryptionFileHandling(string pathWithExt, CipherOptions options)
        {
            if (options.RemoveOriginal)
                Helper.SafeFileDelete(pathWithExt.RemoveDefaultFileExt());
            if (options.MaskFileName)
                File.Move(pathWithExt,
                    pathWithExt = pathWithExt.Replace(Path.GetFileName(pathWithExt).RemoveDefaultFileExt(), Helper.GetRandomAlphanumericString(16)));
            if (options.MaskFileTimes)
                Helper.SetRandomFileTimes(pathWithExt);
        }
    }
}