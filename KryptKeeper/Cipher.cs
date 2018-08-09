using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal static class Cipher
    {
        public const string FILE_EXTENSION = ".krpt";
        public const string WORKING_FILE_EXTENSION = ".krpt.tmp";
        public static BackgroundWorker backgroundWorker;
        private const int CHUNK_SIZE = 64 * 1024 * 1024; // 64MB
        private static readonly Status status = Status.GetInstance();
        private static DateTime cipherStartTime;

        public static void ProcessFiles(CipherOptions options)
        {
            if (options.Files.Length <= 0) return;
            cipherStartTime = DateTime.Now;
            backgroundWorker.RunWorkerAsync(options);
        }

        public static void SetBackgroundWorker(BackgroundWorker bgWorker)
        {
            backgroundWorker = bgWorker;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var options = (CipherOptions)e.Argument;
                for (int i = 0; i < options.Files.Length; i++)
                {
                    if (backgroundWorker.CancellationPending)
                        break;
                    var path = options.Files[i];
                    //backgroundWorker.ReportProgress(i + 1, options.Files.Length);
                    if (!File.Exists(path))
                    {
                        status.WriteLine("* Unable to find: " + path);
                        continue;
                    }
                    status.WritePending($"{options.GetCipherModeOfOperation()}: " + path);
                    if (options.Mode == CipherOptions.ENCRYPT)
                        encrypt(path, options);
                    else
                        decrypt(path, options);
                }
            }
            catch (CryptographicException ex)
            {
                Helper.RemoveTempFile(ex.Message);
                status.WriteLine($"*** Invalid password for {Path.GetFileName(ex.Message)}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Helper.RemoveTempFile(ex.Message);
                status.WriteLine($"*** Unable to access {Path.GetFileName(ex.Message)}");
            }
            catch (Exception ex)
            {
                status.WriteLine($"*** Unhandled exception occured: ({ex.Message})" + Environment.NewLine + ex.StackTrace);
            }
        }

        private static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            status.UpdateProgress(e.ProgressPercentage);
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            status.WriteLine("Operation completed. " + Helper.GetSpannedTime(cipherStartTime.Ticks));
            status.UpdateProgress(0);
        }

        private static void decrypt(string path, CipherOptions options)
        {
            try
            {
                if (!File.Exists(path))
                {
                    status.WriteLine("* File not found: " + path);
                    return;
                }
                using (var aes = Aes.Create())
                {
                    if (aes == null)
                    {
                        status.WriteLine("*** Error: Failed to create AES object!");
                        return;
                    }
                    aes.Key = options.Key;
                    aes.IV = extractIV(path);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    var decryptedPath = path.ReplaceLastOccurrence(FILE_EXTENSION, WORKING_FILE_EXTENSION);
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

                    if (backgroundWorker.CancellationPending)
                    {
                        status.WriteLine("");
                        Helper.SafeFileDelete(decryptedPath);
                        return;
                    }
                    if (File.Exists(decryptedPath) && new FileInfo(decryptedPath).Length > 0)
                    {
                        Helper.SafeFileDelete(path);
                        postDecryptionFileHandling(decryptedPath);
                        return;
                    }
                    status.WriteLine("*** Error: Failed to decrypt file: " + path);
                }
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

        private static void encrypt(string path, CipherOptions options)
        {
            try
            {
                if (!File.Exists(path))
                {
                    status.WriteLine("* File not found: " + path);
                    return;
                }
                string pathWithExt = path + FILE_EXTENSION;
                using (var aes = Aes.Create())
                {
                    if (aes == null)
                    {
                        status.WriteLine("*** Error: Failed to create AES object!");
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

                if (backgroundWorker.CancellationPending)
                {
                    Helper.SafeFileDelete(pathWithExt);
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
            if (backgroundWorker.CancellationPending)
                return;
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
                if (backgroundWorker.CancellationPending)
                    break;
                progressBytes += bytesRead;
                backgroundWorker.ReportProgress((int) Math.Round((double) (100 * progressBytes) / totalBytes));
                cryptoStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, bytesRead);
            }
        }

        private static void postDecryptionFileHandling(string decryptedPath)
        {
            var footer = new Footer();
            if (!footer.TryExtract(decryptedPath))
                throw new Exception("Unable to generate file information from " + decryptedPath);
            using (var fOpen = new FileStream(decryptedPath, FileMode.Open))
            {
                fOpen.SetLength(fOpen.Length - footer.ToArray().Length);
            }
            var projectedPath = decryptedPath.Replace(Path.GetFileName(decryptedPath), footer.Name).Replace(WORKING_FILE_EXTENSION, "");
            if (File.Exists(projectedPath))
                projectedPath = Helper.RenameExistingFile(projectedPath);
            File.Move(decryptedPath, projectedPath);
            Helper.SetFileTimesFromFooter(projectedPath, footer);
        }

        private static void postEncryptionFileHandling(string pathWithExt, CipherOptions options)
        {
            if (options.RemoveOriginal)
                Helper.SafeFileDelete(pathWithExt.RemoveFileExt());
            if (options.MaskFileName)
                File.Move(pathWithExt,
                    pathWithExt = pathWithExt.Replace(Path.GetFileName(pathWithExt).RemoveFileExt(), Helper.GetRandomAlphanumericString(16)));
            if (options.MaskFileTimes)
                Helper.SetRandomFileTimes(pathWithExt);
        }


    }
}