using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KryptKeeper
{
    internal static class Helper
    {

        public static void RemoveTempFile(string path)
        {
            var tempFile = path.Replace(Cipher.FILE_EXTENSION, "");
            SafeFileDelete(tempFile);
        }

        public static void SafeFileDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception)
            {
                // ignore
            }
        }

        public static string RenameExistingFile(string path)
        {
            int i = 1;
            do
            {
                path = path.Replace(Path.GetExtension(path), $" ({i})" + Path.GetExtension(path));
                i++;
            } while (File.Exists(path));
            return path;
        }

        public static string GetSpannedTime(long ticks)
        {
            var time = TimeSpan.FromTicks(Math.Max(1, DateTime.Now.Ticks - ticks));
            var sb = new StringBuilder();
            if (time.Days > 0)
                sb.Append(time.Days + "d ");
            if (time.Hours > 0)
                sb.Append(time.Hours + "h ");
            if (time.Minutes > 0)
                sb.Append(time.Minutes + "m ");
            if (time.Seconds > 0)
                sb.Append(time.Seconds + "s ");
            if (time.Milliseconds > 0)
                sb.Append(time.Milliseconds + "ms");
            return "(" + sb + ")";
        }

        public static void SetRandomFileTimes(string path)
        {
            File.SetCreationTime(path, getRandomFileTime());
            File.SetLastAccessTime(path, getRandomFileTime());
            File.SetLastWriteTime(path, getRandomFileTime());
        }

        private static DateTime getRandomFileTime()
        {
            const long minTicks = 0x1;
            const long maxTicks = 0x7FFF35F4F06C58F;
            return DateTime.FromFileTime(randomTicks(Math.Max(minTicks, DateTime.MinValue.Ticks), Math.Min(maxTicks, DateTime.MaxValue.Ticks)));
        }

        private static long randomTicks(long min, long max)
        {
            var buffer = new byte[8];
            new Random().NextBytes(buffer);
            long longRand = BitConverter.ToInt64(buffer, 0);
            return Math.Abs(longRand % (max - min)) + min;
        }

        public static void SetFileTimesFromFooter(string path, Footer footer)
        {
            File.SetCreationTime(path, footer.CreationTime);
            File.SetLastAccessTime(path, footer.AccessedTime);
            File.SetLastWriteTime(path, footer.ModifiedTime);
        }

        public static Footer GenerateFooter(string path)
        {
            if (File.Exists(path)) throw new FileNotFoundException(path);

            var name = Path.GetFileName(path);
            var created = File.GetCreationTime(path);
            var modified = File.GetLastWriteTime(path);
            var accessed = File.GetLastAccessTime(path);

            return new Footer
            {
                Name = name,
                CreationTime = created,
                ModifiedTime = modified,
                AccessedTime = accessed
            };
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

        public static void ShowErrorBox(string msg)
        {
            MessageBox.Show(msg, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}