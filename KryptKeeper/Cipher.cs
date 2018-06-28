using System;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    public enum CipherMode
    {
        AES,
        RIJNDAEL,
        DES,
        RC2,
        TRIPLEDES
    }

    internal class Cipher
    {
        public static byte[] Encrypt(CipherOptions options)
        {
            byte[] encrypted;

            var provider = getAlgorithm(options.Mode);

            if (provider == null)
            {
                Console.WriteLine(@"Error: algorithm is null");
                return new byte[0];
            }
            provider.GenerateIV();

            provider.GenerateKey();
            var test = provider.KeySize;
            var encryptor = provider.CreateEncryptor(provider.Key, provider.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(options.Data);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            return encrypted;
        }

        public static string Decrypt(CipherOptions options)
        {
            string plainText;
            var provider = getAlgorithm(options.Mode);
            {
                provider.Key = options.Key;
                provider.IV = options.IV;

                ICryptoTransform decryptor = provider.CreateDecryptor(provider.Key, provider.IV);

                using (var msDecrypt = new MemoryStream(options.Data))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plainText;
        }
        private static SymmetricAlgorithm getAlgorithm(CipherMode mode)
        {
            switch (mode)
            {
                case CipherMode.AES:
                    return new AesManaged();
                case CipherMode.RIJNDAEL:
                    return new RijndaelManaged();
                case CipherMode.DES:
                    return new DESCryptoServiceProvider();
                case CipherMode.RC2:
                    return new RC2CryptoServiceProvider();
                case CipherMode.TRIPLEDES:
                    return new TripleDESCryptoServiceProvider();
                default:
                    return null;
            }
        }
    }
}
