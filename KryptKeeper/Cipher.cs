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
        public const string FILE_EXTENSION = ".krpt";

        public static void Encrypt(string path, CipherOptions options)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);

            var dataFromFile = File.ReadAllBytes(path);

            var footerFromPath = new Footer();
            footerFromPath.Build(path);
            var footer = footerFromPath.ToArray();
            
            Console.WriteLine(@"Generated MD5: " + footerFromPath.MD5);

            var data = new byte[dataFromFile.Length + footer.Length];

            Array.Copy(dataFromFile, 0, data, 0, dataFromFile.Length);
            Array.Copy(footer, 0, data, dataFromFile.Length, footer.Length);

            byte[] IV;
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

            var dataComplete = new byte[IV.Length + encrypted.Length];

            Array.Copy(IV, 0, dataComplete, 0, IV.Length);
            Array.Copy(encrypted, 0, dataComplete, IV.Length, encrypted.Length);

            File.WriteAllBytes(path + FILE_EXTENSION, dataComplete);
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

                // get footer
                var footer = new Footer();
                var footerSignature = Encoding.Default.GetBytes(Footer.FOOTER_TAG);
                for (int i = decrypted.Length - footerSignature.Length; i >= 0; i--)
                {
                    if (decrypted[i] != footerSignature[0]) continue;
                    var read = new byte[footerSignature.Length];
                    Array.Copy(decrypted, i, read, 0, read.Length);
                    if (!read.SequenceEqual(footerSignature)) continue;
                    var footerBytes = new byte[decrypted.Length - i];
                    Array.Copy(decrypted, i, footerBytes, 0, footerBytes.Length);
                    var decoded = Encoding.Default.GetString(footerBytes);
                    footer = Footer.FromString(decoded);
                    Console.WriteLine(footer);
                    Array.Resize(ref decrypted, decrypted.Length - footerBytes.Length);
                    break;
                }

                File.WriteAllBytes(path.Substring(0, path.Length - FILE_EXTENSION.Length), decrypted);
            }
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
