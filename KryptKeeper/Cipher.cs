using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal static class Cipher
    {
        private static readonly Status status = Status.GetInstance();

        public const string FILE_EXTENSION = ".krpt";
        private const int CHUNK_SIZE = 64 * 1024 * 1024; // 64MB
        private static DateTime cipherStartTime;

        public static BackgroundWorker backgroundWorker;

        public static void ProcessFiles(CipherOptions options)
        {
            if (options.Files.Length <= 0) return;
            cipherStartTime = DateTime.Now;
            backgroundWorker.RunWorkerAsync(options);
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var options = (CipherOptions) e.Argument;
                for (int i = 0; i < options.Files.Length; i++)
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        status.WriteLine("Stopping operation.");
                        break;
                    }
                    var path = options.Files[i];
                    if (!File.Exists(path))
                    {
                        status.WriteLine("* Unable to find: " + path);
                        continue;
                    }
                    backgroundWorker.ReportProgress(i + 1, options.Files.Length);
                    status.WritePending($"{options.GetCipherModeOfOperation()}: " + path);
                    if (options.Mode == CipherOptions.ENCRYPT)
                        encrypt(path, options);
                    else
                        decrypt(path, options);
                }
            }
            catch (CryptographicException ex)
            {
                status.WriteLine($"*** Invalid password for {ex.Message}");
            }
            catch (Exception ex)
            {
                status.WriteLine("*** Unhandled exception occured: " + ex.StackTrace + ex.Message);
            }
        }

        private static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            status.UpdateProgress(e.ProgressPercentage, (int)e.UserState);
            
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            status.WriteLine("Operation completed. " + Helper.GetSpannedTime(cipherStartTime.Ticks));
            status.UpdateProgress(0, 0);
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
                using (var aes = Aes.Create())
                {
                    if (aes == null)
                    {
                        status.WriteLine("* Error: Failed to create AES object!");
                        return;
                    }
                    aes.Key = options.Key;
                    aes.IV = options.IV;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (var wStream = new FileStream(path + FILE_EXTENSION, FileMode.CreateNew, FileAccess.Write))
                        {
                            wStream.Write(options.IV, 0, options.IV.Length);
                            using (var cStream = new CryptoStream(wStream, encryptor, CryptoStreamMode.Write))
                            {
                                initializeEncryption(path, rStream, cStream);
                            }
                        }
                    }
                }
                postEncryptionFileHandling(path, options);
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



        private static void initializeEncryption(string path, Stream readStream, Stream cryptoStream)
        {
            var buffer = new byte[CHUNK_SIZE];
            readStream.Seek(0, SeekOrigin.Begin);
            int bytesRead = readStream.Read(buffer, 0, CHUNK_SIZE);
            while (bytesRead > 0)
            {
                cryptoStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, bytesRead);
            }
            var footer = new Footer();
            footer.Build(path);
            var footerData = footer.ToArray();
            cryptoStream.Write(footerData, 0, footerData.Length);
        }

        private static void postEncryptionFileHandling(string path, CipherOptions options)
        {
            if (options.RemoveOriginal)
                Helper.SafeFileDelete(path);
            if (options.MaskFileName)
                File.Move(path + FILE_EXTENSION,
                    path = path.Replace(Path.GetFileName(path), Helper.GetRandomAlphanumericString(16)) + FILE_EXTENSION);
            if (options.MaskFileTimes)
                Helper.SetRandomFileTimes(path);
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
                        status.WriteLine("* Error: Failed to create AES object!");
                        return;
                    }
                    aes.Key = options.Key;
                    aes.IV = extractIV(path);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    var decryptedPath = path.Replace(FILE_EXTENSION, "");
                    var footer = new Footer();
                    using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        if (File.Exists(decryptedPath)) decryptedPath = Helper.RenameExistingFile(decryptedPath);
                        using (var wStream = new FileStream(decryptedPath, FileMode.CreateNew, FileAccess.Write))
                        {
                            using (var cStream = new CryptoStream(wStream, decryptor, CryptoStreamMode.Write))
                            {
                                initializeDecryption(rStream, cStream);
                            }
                        }
                    }
                    Helper.SafeFileDelete(path);
                    postDecryptionFileHandling(footer, decryptedPath);
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

        private static byte[] extractIV(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var bReader = new BinaryReader(fStream))
                {
                    return bReader.ReadBytes(16);
                }
            }
        }

        private static void initializeDecryption(FileStream rStream, CryptoStream cStream)
        {
            var buffer = new byte[CHUNK_SIZE];
            rStream.Seek(16, SeekOrigin.Begin); // 16 to skip IV
            int bytesRead = rStream.Read(buffer, 0, CHUNK_SIZE);
            while (bytesRead > 0)
            {
                if (bytesRead < CHUNK_SIZE)
                    Array.Resize(ref buffer, bytesRead);
                cStream.Write(buffer, 0, buffer.Length);
                bytesRead = rStream.Read(buffer, 0, bytesRead);
            }
        }

        private static void postDecryptionFileHandling(Footer footer, string decryptedPath)
        {
            if (!footer.TryExtract(decryptedPath))
                throw new Exception("Unable to generate file information from " + decryptedPath);
            using (var fOpen = new FileStream(decryptedPath, FileMode.Open))
            {
                fOpen.SetLength(fOpen.Length - footer.ToArray().Length);
            }
            var projectedPath = decryptedPath.Replace(Path.GetFileName(decryptedPath), footer.Name);
            if (!File.Exists(projectedPath))
            {
                File.Move(decryptedPath, projectedPath);
                Helper.SafeFileDelete(decryptedPath);
                Helper.SetFileTimesFromFooter(projectedPath, footer);
            }
            else
                Helper.SetFileTimesFromFooter(decryptedPath, footer);
        }

        public static void SetBackgroundWorker(BackgroundWorker bgWorker)
        {
            backgroundWorker = bgWorker;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }
    }
}