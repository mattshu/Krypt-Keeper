using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KryptKeeper
{
    class Encryptor
    {
        public static void AES()
        {
            try
            {
                string original = "this is some data to encrypt for AES";
                using (var myAES = new AesManaged())
                {
                    myAES.GenerateKey(); // temp 32-bits
                    myAES.GenerateIV(); // temp, strip IV from end of encrypted file 16-bits
                    var encryptor = myAES.CreateEncryptor(Encoding.ASCII.GetBytes("very long key helllooooooooooooo"), Encoding.ASCII.GetBytes("hhisissixteenbyt"));

                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(original);
                            }
                            Console.WriteLine(@"Encrypted: {0}", Encoding.ASCII.GetString(msEncrypt.ToArray()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Error: " + e.Message);
            }
            
        }
        public static void Rijndael()
        {
            try
            {
                string original = "Here is some data to encrypt!";

                using (var myRijndael = new RijndaelManaged())
                {
                    //byte[] key = Encoding.ASCII.GetBytes("testing");
                    myRijndael.GenerateKey();
                    myRijndael.GenerateIV();
                    byte[] encrypted = EncryptStringToBytes(original, myRijndael.Key, myRijndael.IV);
                    string roundtrip = DecryptStringFromBytes(encrypted, myRijndael.Key, myRijndael.IV);
                    Console.WriteLine(@"Original:   {0}", original);
                    Console.WriteLine(@"Encrypted: {0}", Encoding.Default.GetString(encrypted));
                    Console.WriteLine(@"Round Trip: {0}", roundtrip);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(@"Error: {0}", e.Message);
            }
        }
        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;

        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}
