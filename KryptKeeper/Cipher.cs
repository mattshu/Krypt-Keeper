using System;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class Cipher
    {
        public const string FILE_EXTENSION = ".krpt";

        public static void EncryptFiles(string[] files, CipherOptions options)
        {
            if (files.Length <= 0) return;
            foreach (string file in files)
                encrypt(file, options);
        }

        public static void DecryptFiles(string[] files, CipherOptions options)
        {
            if (files.Length <= 0) return;
            foreach (string file in files)
                decrypt(file, options);
        }

        private static void encrypt(string path, CipherOptions options)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            var fileData = File.ReadAllBytes(path);
            var footer = new Footer();
            footer.Build(path);
            var footerData = footer.ToArray();
            var data = Helper.PackData(fileData, footerData);
            var encrypted = encryptData(data, options, out var IV);
            var dataComplete = Helper.PackData(IV, encrypted);
            if (options.RemoveOriginal)
                File.Delete(path);
            if (options.MaskFileName)
                path = path.Replace(Path.GetFileName(path), Helper.GetRandomAlphanumericString(16));
            path = path + FILE_EXTENSION;
            File.WriteAllBytes(path, dataComplete);
            if (options.MaskFileTimes)
                setFileTimes(path);
        }

        private static void decrypt(string path, CipherOptions options)
        {
            var data = File.ReadAllBytes(path);
            using (var provider = getAlgorithm(options.Mode))
            {
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
                setFileTimes(decryptedPath, footer); // Set to original filetimes
                if (Helper.GetMD5StringFromPath(decryptedPath).Equals(footer.MD5))
                    File.Delete(path); // Remove encryption after validation
                else
                    throw new Exception(@"Failed to compare MD5 of " + path + ". File tampered?");
            }
        }

        private static byte[] encryptData(byte[] data, CipherOptions options, out byte[] IV)
        {
            byte[] encrypted;

            using (var provider = getAlgorithm(options.Mode))
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

        private static byte[] decryptData(byte[] encrypted, ICryptoTransform decryptor)
        {
            try
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
            } catch (CryptographicException cryptoException)
            {
                throw new Exception(@"Unable to decrypt data, file may have been tampered with. \n" + cryptoException.Message);
            }
        }

        private static void setFileTimes(string encryptedPath)
        {
            // Prepending "130" keeps the random date (arguably) more believable
            File.SetCreationTime(encryptedPath,
                DateTime.FromFileTime(long.Parse("130" + Helper.GetRandomNumericString(15))));
            File.SetLastAccessTime(encryptedPath,
                DateTime.FromFileTime(long.Parse("130" + Helper.GetRandomNumericString(15))));
            File.SetLastWriteTime(encryptedPath,
                DateTime.FromFileTime(long.Parse("130" + Helper.GetRandomNumericString(15))));
        }

        private static void setFileTimes(string decryptedPath, Footer footer)
        {
            File.SetCreationTime(decryptedPath, footer.CreationTime);
            File.SetLastAccessTime(decryptedPath, footer.AccessedTime);
            File.SetLastWriteTime(decryptedPath, footer.ModifiedTime);
        }

        private static SymmetricAlgorithm getAlgorithm(CipherAlgorithm mode)
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
                    throw new Exception("Unknown algorithm: " + mode); // TODO new Exception
            }
        }
    }
}