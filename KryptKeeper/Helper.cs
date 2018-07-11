using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace KryptKeeper
{
    internal static class Helper
    {
        public static byte[] PackData(byte[] dataA, byte[] dataB)
        {
            var data = new byte[dataA.Length + dataB.Length];
            Array.Copy(dataA, 0, data, 0, dataA.Length); // Copy dataA into data
            Array.Copy(dataB, 0, data, dataA.Length, dataB.Length); // Copy dataB into data
            return data;
        }

        public static Footer GenerateFooter(string path)
        {
            if (File.Exists(path)) throw new FileNotFoundException(path);

            var md5 = GetMD5StringFromPath(path);
            var name = Path.GetFileName(path);
            var created = File.GetCreationTime(path);
            var modified = File.GetLastWriteTime(path);
            var accessed = File.GetLastAccessTime(path);

            return new Footer
            {
                MD5 = md5,
                Name = name,
                CreationTime = created,
                ModifiedTime = modified,
                AccessedTime = accessed
            };
        }

        public static string GetMD5StringFromPath(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            using (var md5 = MD5.Create())
            {
                using (var fstream = File.OpenRead(path))
                {
                    return BitConverter.ToString(md5.ComputeHash(fstream)).Replace("-", "");
                }
            }
        }

        public static string GetMD5ToString(byte[] md5Data)
        {
            return BitConverter.ToString(md5Data).Replace("-", "");
        }

        public static string GetRandomAlphanumericString(int length)
        {
            if (length <= 0) return "";
            var random = new byte[length];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);
            return BitConverter.ToString(random).Replace("-", "").Substring(0, length);
        }

        public static string GetRandomNumericString(int length)
        {
            return Regex.Replace(GetRandomAlphanumericString(length), @"[A-F]", "0");
        }
    }
}