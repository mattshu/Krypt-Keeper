using System;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class Cipher
    {
        private static readonly Status status = Status.GetInstance();

        public const string FILE_EXTENSION = ".krpt";
        private const int CHUNK_SIZE = 64 * 1024 * 1024; // 64MB
        private static DateTime encryptionStartTime;
        private static DateTime decryptionStartTime;

        public static void EncryptFiles(string[] files, CipherOptions options)
        {
            encryptionStartTime = DateTime.Now;
            if (files.Length <= 0) return;
            foreach (var file in files)
                encrypt(file, options);
            status.WriteLine("Encryption completed. " + Helper.GetSpannedTime(encryptionStartTime.Ticks));
        }

        public static void DecryptFiles(string[] files, CipherOptions options)
        {
            decryptionStartTime = DateTime.Now;
            if (files.Length <= 0) return;
            foreach (var file in files)
                decrypt(file, options);
            status.WriteLine("Decryption completed. " + Helper.GetSpannedTime(decryptionStartTime.Ticks));
        }

        private static void encrypt(string path, CipherOptions options)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException(path);
                status.WritePending("Encrypting: " + path);

                using (var aes = Aes.Create())
                {
                    if (aes == null) throw new CryptographicException("Failed to create AES object!");
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
            catch (FileNotFoundException ex)
            {
                status.WriteLine("*** Error: File not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                status.WriteLine("*** Error occured: " + ex.StackTrace + ex.Message);
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
                File.Delete(path); // TODO handle UnauthorizedAccessException
            if (options.MaskFileName)
                File.Move(path + FILE_EXTENSION,
                    path = path.Replace(Path.GetFileName(path), Helper.GetRandomAlphanumericString(16)) + FILE_EXTENSION);
            if (options.MaskFileTimes)
                Helper.SetRandomFileTimes(path);
        }

        private static void decrypt(string path, CipherOptions options)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            status.WritePending("Decrypting: " + path);
            using (var aes = Aes.Create())
            {
                if (aes == null) throw new CryptographicException("Failed to create AES object!");
                aes.Key = options.Key;
                aes.IV = extractIV(path); // TODO throws CryptographicException if wrong decryption method
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var decryptedPath = path.Replace(FILE_EXTENSION, "");
                var footer = new Footer();
                using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (File.Exists(decryptedPath))
                        decryptedPath = renameFileWithPadding(decryptedPath);
                    using (var wStream = new FileStream(decryptedPath, FileMode.CreateNew, FileAccess.Write))
                    {
                        using (var cStream = new CryptoStream(wStream, decryptor, CryptoStreamMode.Write)) // TODO throws CryptographicException: 'Padding is invalid and cannot be removed.' if invalid file
                        {
                            initializeDecryption(rStream, cStream);
                        }
                    }
                }
                File.Delete(path);
                postDecryptionFileHandling(footer, decryptedPath);
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
                File.Delete(decryptedPath);
                Helper.SetFileTimesFromFooter(projectedPath, footer);
            }
            else
                Helper.SetFileTimesFromFooter(decryptedPath, footer);
        }

        private static string renameFileWithPadding(string newOriginalPath)
        {
            string newPath;
            do
            {
                newPath = addRandomPaddingToFileName(newOriginalPath);
            } while (File.Exists(newPath));
            status.WriteLine("*** File already exists (" + newOriginalPath + ") Renaming to: " + newPath);
            newOriginalPath = newPath;
            return newOriginalPath;
        }

        private static string addRandomPaddingToFileName(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            return path.Replace(fileName ?? "", fileName + Helper.GetRandomAlphanumericString(5));
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
    }
}