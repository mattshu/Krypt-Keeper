using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class CipherOptions
    {
        public string[] Files { get; set; }
        public bool MaskFileName { get; set; }
        public bool MaskFileTimes { get; set; }
        public bool RemoveOriginal { get; set; }
        public int Mode { get; set; }
        public byte[] IV { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Key { get; set; }

        public long CalculateTotalPayload()
        {
            return Files.Length > 0 ? Files.Sum(f => new FileInfo(f).Length) : 0;
        }

        public void GenerateIV()
        {
            using (var aes = Aes.Create())
            {
                if (aes == null) throw new CryptographicException("Failed to create AES object!");
                aes.GenerateIV();
                IV = aes.IV;
            }
        }

        public string GetModeOfOperation() => Mode == Cipher.ENCRYPT ? "Encrypting" : "Decrypting";
    }
}