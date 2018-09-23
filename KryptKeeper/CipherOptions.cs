using System.Security.Cryptography;

namespace KryptKeeper
{
    public class CipherOptions
    {
        public FileList Files { get; set; }
        public Cipher.Mode Mode { get; set; }
        public bool MaskFileName { get; set; }
        public bool MaskFileDate { get; set; }
        public bool RemoveOriginalDecryption { get; set; }
        public bool RemoveOriginalEncryption { get; set; }
        public byte[] IV { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Key { get; set; }
        public byte[] Entropy { get; private set; }

        public string GetModeOfOperation() => Mode == Cipher.Mode.Encrypt ? "Encrypting" : "Decrypting";

        public CipherOptions()
        {
            generateEntropy();
            generateIV();
        }

        private void generateEntropy()
        {
            var entropy = new byte[15];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            if (entropy.Length > 0)
                Entropy = entropy;
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
    }
}