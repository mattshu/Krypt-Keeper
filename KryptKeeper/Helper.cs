using KryptKeeper.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace KryptKeeper
{
    internal static class Helper
    {
        public static long CalculateTotalPayloadBytes(this List<FileData> fileData)
        {
            return fileData.Count > 0 ? fileData.Sum(f => new FileInfo(f.GetFilePath()).Length) : 0;
        }

        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return @"0";
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            if (place < 1) return @"1 KB";
            var num = Math.Round(bytes / Math.Pow(1024, place));
            return Math.Sign(byteCount) * num + " " + suf[place];
        }

        public static string BrowseFiles(bool multiSelect = true)
        {
            var openFile = new OpenFileDialog { Multiselect = multiSelect, CheckFileExists = true };
            return openFile.ShowDialog() != DialogResult.OK ? "" : openFile.FileName;
        }

        public static byte[] GenerateLogHeader()
        {
            var timestamp = DateTime.Now;
            var header = $"KryptKeeper v{Application.ProductVersion}{Environment.NewLine}Log generated on {timestamp}{Environment.NewLine}";
            return GetBytes(header);
        }

        public static byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static int GetPercentProgress(long current, long total)
        {
            return (int)Math.Round((double)(100 * current) / total);
        }

        public static string GetRandomAlphanumericString(int length)
        {
            if (length <= 0) return "";
            var random = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(random);
                return BitConverter.ToString(random).Replace("-", "").Substring(0, length);
            }
        }

        public static byte[] GetSHA256(byte[] value)
        {
            using (var sha = SHA256.Create())
                return sha.ComputeHash(value);
        }

        public static string GetSpannedTime(long ticks, bool hideMs = false)
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
            if (!hideMs && time.Milliseconds > 0)
                sb.Append(time.Milliseconds + "ms");
            return sb.ToString();
        }

        public static string PadExistingFileName(string fullPath)
        {
            var count = 1;
            var path = Path.GetDirectoryName(fullPath);
            var fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            var extension = Path.GetExtension(fullPath);
            var newFullPath = fullPath;
            while (File.Exists(newFullPath))
            {
                var tempFileName = $"{fileNameOnly} ({count++})";
                newFullPath = Path.Combine(path ?? "", tempFileName + extension);
            }
            return newFullPath;
        }

        public static string RemoveDefaultFileExt(this string path)
        {
            return path.Replace(Cipher.WORKING_FILE_EXTENSION, "").Replace(Cipher.FILE_EXTENSION, "");
        }

        public static string ReplaceLastOccurrence(this string source, string find, string replace)
        {
            var place = source.LastIndexOf(find, StringComparison.Ordinal);
            if (place == -1)
                return source;
            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static void ResetSettings()
        {
            var settings = Settings.Default;
            settings.cipherKeyType = 0;
            settings.encryptionMaskFileName = false;
            settings.encryptionMaskFileDate = false;
            settings.removeAfterEncryption = true;
            settings.removeAfterDecryption = true;
            settings.rememberSettings = false;
            settings.confirmOnExit = true;
            settings.fileListColumnOrder = new StringCollection { "1", "3", "0", "2" };
            settings.fileListColumnWidths = new StringCollection { "72", "194", "164", "103" };
            settings.Save();
        }

        public static void SetFileTimesFromFooter(string path, Footer footer)
        {
            File.SetCreationTime(path, footer.CreationTime);
            File.SetLastAccessTime(path, footer.AccessedTime);
            File.SetLastWriteTime(path, footer.ModifiedTime);
        }

        public static void SetRandomFileTimes(string path)
        {
            File.SetCreationTime(path, getRandomFileTime());
            File.SetLastAccessTime(path, getRandomFileTime());
            File.SetLastWriteTime(path, getRandomFileTime());
        }

        public static void ShowErrorBox(string msg)
        {
            MessageBox.Show(msg, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool TryDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
    }
}