using System;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class Cipher
    {
        public const string FILE_EXTENSION = ".krpt";

        public static void Encrypt(string path, CipherOptions options)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            var dataFromFile = File.ReadAllBytes(path);
            var footer = new Footer();
            footer.Build(path);
            var footerData = footer.ToArray();
            var data = new byte[dataFromFile.Length + footerData.Length];
            Array.Copy(dataFromFile, 0, data, 0, dataFromFile.Length); // Copy dataFromFile into data
            Array.Copy(footerData, 0, data, dataFromFile.Length, footerData.Length); // Copy footer into data
            var encrypted = EncryptData(options, data, out var IV);
            var dataComplete = new byte[IV.Length + encrypted.Length];
            Array.Copy(IV, 0, dataComplete, 0, IV.Length); // Copy IV to finished package
            Array.Copy(encrypted, 0, dataComplete, IV.Length, encrypted.Length); // Copy encrypted data to finished package
            if (options.MaskFileName)
            {
                var newPath = path.Replace(Path.GetFileName(path), Helper.GetRandomAlphanumericString(16));
                File.Delete(path);
                path = newPath;
            }
            path = path + FILE_EXTENSION;
            File.WriteAllBytes(path, dataComplete);
            if (options.MaskFileTimes)
                SetFileTimes(path);
        }

        private static byte[] EncryptData(CipherOptions options, byte[] data, out byte[] IV)
        {
            byte[] encrypted;

            using (var provider = GetAlgorithm(options.Mode))
            {
                provider.Key = options.Key;
                provider.GenerateIV();
                IV = provider.IV;
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                var encryptor = provider.CreateEncryptor(provider.Key, provider.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new BinaryWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public static void Decrypt(string path, CipherOptions options)
        {
            var data = File.ReadAllBytes(path);
            using (var provider = GetAlgorithm(options.Mode))
            {
                provider.Key = options.Key;
                var IV = new byte[provider.BlockSize / 8];
                var encrypted = new byte[data.Length - IV.Length];
                Array.Copy(data, IV, IV.Length);
                Array.Copy(data, IV.Length, encrypted, 0, encrypted.Length);
                provider.IV = IV;
                provider.Mode = CipherMode.CBC;
                var decryptor = provider.CreateDecryptor(provider.Key, provider.IV);
                var decrypted = DecryptData(decryptor, encrypted);
                var footer = new Footer();
                if (!footer.Extract(decrypted))
                    throw new Exception(@"Failed to extract footer of " + path +". File corrupt?");
                var footerBytes = footer.ToArray();
                Array.Resize(ref decrypted, decrypted.Length - footerBytes.Length); // Trim footer
                string decryptedPath = path.Substring(0, path.Length - FILE_EXTENSION.Length); // Trim extension
                if (!string.IsNullOrEmpty(footer.Name))
                    decryptedPath = decryptedPath.Replace(Path.GetFileName(decryptedPath), footer.Name); // Set to original filename
                File.WriteAllBytes(decryptedPath, decrypted);
                SetFileTimes(decryptedPath, footer); // Set to original filetimes
                if (Helper.GetMD5StringFromPath(decryptedPath).Equals(footer.MD5))
                    File.Delete(path); // Remove encryption after validation
                else
                    throw new Exception(@"Failed to compare MD5 of " + path + ". File tampered?");
            }
        }

        private static byte[] DecryptData(ICryptoTransform decryptor, byte[] encrypted)
        {
            byte[] decrypted;
            using (var msDecrypt = new MemoryStream(encrypted))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new BinaryReader(csDecrypt))
                    {
                        decrypted = srDecrypt.ReadBytes(encrypted.Length);
                    }
                }
            }
            return decrypted;
        }

        private static void SetFileTimes(string encryptedPath)
        {
            // Prepending "130" keeps the random date (arguably) more believable
            File.SetCreationTime(encryptedPath, DateTime.FromFileTime(long.Parse("130" + Helper.GetRandomNumericString(15))));
            File.SetLastAccessTime(encryptedPath, DateTime.FromFileTime(long.Parse("130" + Helper.GetRandomNumericString(15))));
            File.SetLastWriteTime(encryptedPath, DateTime.FromFileTime(long.Parse("130" + Helper.GetRandomNumericString(15))));
        }
        private static void SetFileTimes(string decryptedPath, Footer footer)
        {
            File.SetCreationTime(decryptedPath, footer.CreationTime);
            File.SetLastAccessTime(decryptedPath, footer.AccessedTime);
            File.SetLastWriteTime(decryptedPath, footer.ModifiedTime);
        }

        private static SymmetricAlgorithm GetAlgorithm(CipherAlgorithm mode)
        {
            switch (mode)
            {
                case CipherAlgorithm.AES:
                    return new AesManaged();
                case CipherAlgorithm.RIJNDAEL:
                    return new RijndaelManaged();
                case CipherAlgorithm.DES:
                    return new DESCryptoServiceProvider();
                case CipherAlgorithm.RC2:
                    return new RC2CryptoServiceProvider();
                case CipherAlgorithm.TRIPLEDES:
                    return new TripleDESCryptoServiceProvider();
                default:
                    throw new Exception("Unknown algorithm: " + mode);
            }
        }
    }
}
