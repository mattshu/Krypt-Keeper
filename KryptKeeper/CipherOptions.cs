using System;
using System.Linq;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class CipherOptions
    {
        public CipherMode Mode { get; set; }
        public byte[] Key
        {
            get => key;
            set
            {
                key = value;
                if (key.Length < 128)
                {
                    var keyToList = key.ToList();
                    var padding = new byte[128 - key.Length];
                    keyToList.AddRange(padding.ToList());
                    padding = new byte[128];
                    keyToList.AddRange(padding.ToList());
                    key = keyToList.ToArray();
                }
                else if (key.Length > 256)
                {
                    key = key.ToList().Take(128).ToArray();
                }
            }
        }
        private byte[] key;
        public byte[] IV { get; set; } = null;
        public byte[] Data { get; set; }
    }
}
