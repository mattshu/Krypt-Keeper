using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KryptKeeper
{
    internal static class Helper
    {

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
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetRandomNumericString(int length)
        {
            if (length <= 0) return "";
            var random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
