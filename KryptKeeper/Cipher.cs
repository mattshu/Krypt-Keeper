using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KryptKeeper
{
    internal class Cipher
    {
        public const string DEFAULT_EXTENSION = ".krpt";

        public static void Encrypt(string path, CipherOptions options)
        {
            var data = File.ReadAllBytes(path);

            byte[] encrypted;
            byte[] IV;
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

            var dataWithIV = new byte[IV.Length + encrypted.Length];
            Array.Copy(IV, 0, dataWithIV, 0, IV.Length);
            Array.Copy(encrypted, 0, dataWithIV, IV.Length, encrypted.Length);
            File.WriteAllBytes(path + DEFAULT_EXTENSION, dataWithIV);
        }

        public static void Decrypt(string path, CipherOptions options)
        {
            var data = File.ReadAllBytes(path);

            using (var provider = getAlgorithm(options.Mode))
            {
                provider.Key = options.Key;

                byte[] IV = new byte[provider.BlockSize / 8];
                byte[] encrypted = new byte[data.Length - IV.Length];

                Array.Copy(data, IV, IV.Length);
                Array.Copy(data, IV.Length, encrypted, 0, encrypted.Length);

                provider.IV = IV;
                provider.Mode = CipherMode.CBC;
                var decryptor = provider.CreateDecryptor(provider.Key, provider.IV);

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

                File.WriteAllBytes(path.Substring(0, path.Length - DEFAULT_EXTENSION.Length), decrypted);
            }
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
                    throw new Exception("Unknown algorithm: " + mode);
            }
        }
    }
}
