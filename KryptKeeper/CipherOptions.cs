﻿using System.Linq;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class CipherOptions
    {
        public CipherAlgorithm Mode { get; set; } = CipherAlgorithm.AES;
        public byte[] Key
        {
            get => key;
            set => key = Mode == CipherAlgorithm.DES ? getMD5(value).Take(8).ToArray() : getMD5(value);
        }
        private byte[] key;
        public bool MaskFileName { get; set; }
        public bool MaskFileTimes { get; set; }
        public bool RemoveOriginal { get; set; }
        public static int Encrypt { get; } = 0;
        public static int Decrypt { get; } = 1;

        private static byte[] getMD5(byte[] value)
        {
            return new MD5CryptoServiceProvider().ComputeHash(value);
        }
    }
}