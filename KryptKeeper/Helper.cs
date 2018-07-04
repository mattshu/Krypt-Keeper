using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KryptKeeper
{
    internal static class Helper
    {

        public static Footer GenerateFooter(string path)
        {
            if (File.Exists(path)) throw new FileNotFoundException(path);

            var md5 = GetMD5FromPath(path);
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

        public static byte[] GetMD5FromPath(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            using (var md5 = MD5.Create())
            using (var fstream = File.OpenRead(path))
                return md5.ComputeHash(fstream);
        }

        public static string GetMD5ToString(byte[] md5Data)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return BitConverter.ToString(md5.ComputeHash(md5Data)).Replace("-", "");
        }

        public static byte[] GetMD5FromString(string md5String)
        {
            return Encoding.Default.GetBytes(md5String.ToUpper());
        }

            /* Byte array search */

        private static readonly int[] Empty = new int[0];
        public static int[] Locate(this byte[] self, byte[] candidate)
        {
            if (IsEmptyLocate(self, candidate)) return Empty;
            var list = new List<int>();
            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate)) continue;
                list.Add(i);
            }
            return list.Count == 0 ? Empty : list.ToArray();
        }

        private static bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > array.Length - position)
                return false;
            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;
            return true;
        }

        private static bool IsEmptyLocate(byte[] array, byte[] candidate)
        {
            return array == null
                   || candidate == null
                   || array.Length == 0
                   || candidate.Length == 0
                   || candidate.Length > array.Length;
        }
    }
}
