using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class CipherOptions
    {
        public byte[] Key
        {
            get => key;
            set => key = getMD5(value);
        }
        private byte[] key;
        public byte[] IV { get; set; }
        public bool MaskFileName { get; set; }
        public bool MaskFileTimes { get; set; }
        public bool RemoveOriginal { get; set; }
        public static int Encrypt { get; } = 0;
        public static int Decrypt { get; } = 1;

        public void GenerateIV()
        { 
            using (var aes = Aes.Create())
            {
                if (aes == null) throw new CryptographicException("Failed to create AES object!");
                aes.GenerateIV();
                IV = aes.IV;
            }
        }

        private static byte[] getMD5(byte[] value)
        {
            
            return MD5.Create().ComputeHash(value);
        }
    }
}