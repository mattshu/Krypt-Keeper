using System.Collections.Generic;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class CipherOptions
    {
        public List<FileData> Files { get; set; }
        public bool MaskFileName { get; set; }
        public bool MaskFileDate { get; set; }
        public bool RemoveOriginalDecryption { get; set; }
        public bool RemoveOriginalEncryption { get; set; }
        public int Mode { get; set; }
        public byte[] IV { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Key { get; set; }
        public byte[] SecureRandomFiller { get; private set; }

        public CipherOptions()
        {
            generateSecureRandomFiller();
            generateIV();
        }

        private void generateIV()
        {
            using (var aes = Aes.Create())
            {
                if (aes == null) throw new CryptographicException("Failed to create AES object!");
                aes.GenerateIV();
                IV = aes.IV;
            }
        }

        private void generateSecureRandomFiller()
        {
            var entropy = new byte[15];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            if (entropy.Length > 0)
                SecureRandomFiller = entropy;
        }

        public string GetModeOfOperation() => Mode == Cipher.ENCRYPT ? "Encrypting" : "Decrypting";
    }
}