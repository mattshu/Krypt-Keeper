﻿using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class CipherOptions
    {
        public string[] Files { get; set; }
        public byte[] IV { get; set; }
        public byte[] Key
        {
            get => key;
            set => key = getMD5(value);
        }
        private byte[] key;
        public bool MaskFileName { get; set; }
        public bool MaskFileTimes { get; set; }
        public int Mode { get; set; }
        public bool RemoveOriginal { get; set; }

        public void GenerateIV()
        {
            using (var aes = Aes.Create())
            {
                if (aes == null) throw new CryptographicException("Failed to create AES object!");
                aes.GenerateIV();
                IV = aes.IV;
            }
        }

        public string GetModeOfOperation()
        {
            return Mode == Cipher.ENCRYPT ? "Encryption" : "Decryption";
        }

        private static byte[] getMD5(byte[] value)
        {
            return MD5.Create().ComputeHash(value);
        }
    }
}