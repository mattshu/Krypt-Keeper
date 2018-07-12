using System;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class Cipher
    {
        private static readonly Status status = Status.GetInstance();

        public const string FILE_EXTENSION = ".krpt";

        public static void EncryptFiles(string[] files, CipherOptions options)
        {
            if (files.Length <= 0) return;
            foreach (string file in files)
                encrypt(file, options);
            status.WriteLine("Encryption completed.");
        }

        public static void DecryptFiles(string[] files, CipherOptions options)
        {
            if (files.Length <= 0) return;
            foreach (string file in files)
                decrypt(file, options);
            status.WriteLine("Decryption completed.");
        }

        private static void encrypt(string path, CipherOptions options)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            status.WritePending("Encrypting: " + path);
            const int chunkSize = 1024; // 1KB
            using (var provider = Helper.GetAlgorithm(options.Mode))
            {
                provider.Key = options.Key;
                provider.IV = options.IV;
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                var encryptor = provider.CreateEncryptor(provider.Key, provider.IV);
                using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var wStream = new FileStream(path + FILE_EXTENSION, FileMode.CreateNew, FileAccess.Write))
                    {
                        wStream.Write(options.IV, 0, options.IV.Length);
                        using (var cStream = new CryptoStream(wStream, encryptor, CryptoStreamMode.Write))
                        {
                            var buffer = new byte[chunkSize];
                            rStream.Seek(0, SeekOrigin.Begin);
                            int bytesRead = rStream.Read(buffer, 0, chunkSize);
                            while (bytesRead > 0)
                            {
                                if (bytesRead < 1024)
                                    Array.Resize(ref buffer, bytesRead);
                                cStream.Write(buffer, 0, buffer.Length);
                                bytesRead = rStream.Read(buffer, 0, bytesRead);
                            }
                            var footer = new Footer();
                            footer.Build(path);
                            var footerData = footer.ToArray();
                            cStream.Write(footerData, 0, footerData.Length);
                        }
                    }
                }
            }
            if (options.RemoveOriginal)
                File.Delete(path);
            if (options.MaskFileName)
                File.Move(path + FILE_EXTENSION,
                    path.Replace(Path.GetFileName(path), Helper.GetRandomAlphanumericString(16)) + FILE_EXTENSION);
        }

        private static void decrypt(string path, CipherOptions options)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            status.WritePending("Decrypting: " + path);
            const int chunkSize = 1024; // 1KB
            using (var provider = Helper.GetAlgorithm(options.Mode))
            {
                provider.Key = options.Key;
                provider.IV = options.IV;
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                var encryptor = provider.CreateDecryptor(provider.Key, provider.IV);
                using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var wStream = new FileStream(path.Replace(FILE_EXTENSION, ""), FileMode.CreateNew, FileAccess.Write))
                    {
                        //wStream.Write(options.IV, 0, options.IV.Length);
                        using (var cStream = new CryptoStream(wStream, encryptor, CryptoStreamMode.Write))
                        {
                            var buffer = new byte[chunkSize];
                            rStream.Seek(16, SeekOrigin.Begin); // 16 to skip IV
                            int bytesRead = rStream.Read(buffer, 0, chunkSize);
                            while (bytesRead > 0)
                            {
                                if (bytesRead < 1024)
                                    Array.Resize(ref buffer, bytesRead);
                                cStream.Write(buffer, 0, buffer.Length);
                                bytesRead = rStream.Read(buffer, 0, bytesRead);
                            }
                            var footer = new Footer();
                            /* TODO
                             * footer.Extract(path);
                            Helper.SetFileTimes(decryptedPath, footer); // Set to original filetimes
                            if (Helper.GetMD5StringFromPath(decryptedPath).Equals(footer.MD5))
                                File.Delete(path); // Remove encryption after validation
                            else
                                throw new Exception(@"Failed to compare MD5 of " + path + ". File tampered?");
                                */
                        }
                    }
                }
            }
        }

        private static void decryptxxx(string path, CipherOptions options)
        {
            var data = File.ReadAllBytes(path);
            using (var provider = Helper.GetAlgorithm(options.Mode))
            {
                status.WritePending("Decrypting: " + path);
                provider.Key = options.Key;
                var IV = new byte[provider.BlockSize / 8];
                var encrypted = new byte[data.Length - IV.Length];
                Array.Copy(data, IV, IV.Length);
                Array.Copy(data, IV.Length, encrypted, 0, encrypted.Length);
                provider.IV = IV;
                provider.Mode = CipherMode.CBC;
                var decryptor = provider.CreateDecryptor(provider.Key, provider.IV);
                var decrypted = decryptData(encrypted, decryptor);
                var footer = new Footer();
                if (!footer.Extract(decrypted))
                    throw new Exception(@"Failed to extract footer of " + path + ". File corrupt?");
                var footerBytes = footer.ToArray();
                Array.Resize(ref decrypted, decrypted.Length - footerBytes.Length); // Trim footer
                var decryptedPath = path.Substring(0, path.Length - FILE_EXTENSION.Length); // Trim extension
                if (!string.IsNullOrEmpty(footer.Name))
                    decryptedPath =
                        decryptedPath.Replace(Path.GetFileName(decryptedPath), footer.Name); // Set to original filename
                File.WriteAllBytes(decryptedPath, decrypted);
                Helper.SetFileTimes(decryptedPath, footer); // Set to original filetimes
                if (Helper.GetMD5StringFromPath(decryptedPath).Equals(footer.MD5))
                    File.Delete(path); // Remove encryption after validation
                else
                    throw new Exception(@"Failed to compare MD5 of " + path + ". File tampered?");
            }
        }

        private static byte[] decryptData(byte[] encrypted, ICryptoTransform decryptor)
        {
            try
            {
                byte[] decrypted;
                using (var msDecrypt = new MemoryStream(encrypted))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var brDecrypt = new BinaryReader(csDecrypt))
                        {
                            decrypted = brDecrypt.ReadBytes(encrypted.Length);
                        }
                    }
                }
                return decrypted;
            } catch (CryptographicException cryptoException)
            {
                throw new Exception(@"Unable to decrypt data, file may have been tampered with. \n" + cryptoException.Message);
            }
        }
    }
}