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
            var provider = getAlgorithm(options.Mode);
            provider.GenerateIV();
            provider.Mode = CipherMode.CBC;
            provider.Padding = PaddingMode.PKCS7;
            var encryptor = provider.CreateEncryptor(options.Key, provider.IV);
            var data = File.ReadAllBytes(path);
            using (var fStream = new FileStream(path + DEFAULT_EXTENSION, FileMode.Create))
            {
                using (var cryptStream = new CryptoStream(fStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var output = new BinaryWriter(cryptStream))
                    {
                        //output.Write(encryptor.TransformFinalBlock(data, 0, data.Length));
                        output.Write(data);
                        //output.Write(fileFooter);
                        output.Write(provider.IV);
                    }
                }
            }
            // TODO if (File.Exists(path + DEFAULT_EXTENSION)) handle;
            /*using (var fStream = new FileStream(path + DEFAULT_EXTENSION, FileMode.Create))
            {
                using (var output = new BinaryWriter(fStream, Encoding.Default))
                {
                    output.Write(encryptor.TransformFinalBlock(data, 0, data.Length));
                    //output.Write(fileFooter);
                    output.Write(provider.IV);
                }
            }*/
        } 

        public static void Decrypt(string path, CipherOptions options)
        {
            var rawData = File.ReadAllBytes(path);
            var data = new ArraySegment<byte>(rawData, 0, rawData.Length - 16).ToArray();
            var IV = new ArraySegment<byte>(rawData, rawData.Length - 16, 16).ToArray();
            var provider = getAlgorithm(options.Mode);
            provider.Mode = CipherMode.CBC;
            provider.Padding = PaddingMode.PKCS7;
            using (var mStream = new MemoryStream())
            {
                using (var cStream = new CryptoStream(mStream, provider.CreateDecryptor(options.Key, IV), CryptoStreamMode.Write))
                {
                    cStream.Write(data, 0, data.Length);
                } // PADDING INVALID?>!!? TODO **
                Console.WriteLine("Decrypted: " + Encoding.UTF8.GetString(mStream.ToArray()));
            }
            
            /*var decryptor = provider.CreateDecryptor(options.Key, options.Mode == CipherAlgorithm.DES ? IV.Take(8).ToArray() : IV);
            var decryptedData = decryptor.TransformFinalBlock(data, 0, data.Length);
            using (var fStream = new FileStream(path.Substring(0, path.Length - DEFAULT_EXTENSION.Length), FileMode.Create))
            {
                using (var output = new BinaryWriter(fStream, Encoding.Default))
                {
                    output.Write(decryptedData);
                }
            }*/
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
