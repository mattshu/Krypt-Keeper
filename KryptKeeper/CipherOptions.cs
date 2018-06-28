using System;
using System.Linq;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class CipherOptions
    {
        public CipherAlgorithm Mode { get; set; } = CipherAlgorithm.AES;
        public byte[] Key
        {
            get => key;
            set => key = Mode == CipherAlgorithm.DES ? _getMD5(value).Take(8).ToArray() : _getMD5(value);
        }
        private byte[] key;

        private static byte[] _getMD5(byte[] value)
        {
            return new MD5CryptoServiceProvider().ComputeHash(value);
        }
    }
}
