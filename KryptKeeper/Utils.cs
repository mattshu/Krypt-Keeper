using KryptKeeper.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KryptKeeper
{
    internal static class Utils
    {
        public static string BrowseFiles(string title = "Select a file", bool multiSelect = true)
        {
            var openFile = new OpenFileDialog { Title = title, Multiselect = multiSelect, CheckFileExists = true };
            return openFile.ShowDialog() != DialogResult.OK ? "" : openFile.FileName;
        }

        public static string BytesToSizeString(this long byteCount, bool limitToKB = false)
        {
            if (byteCount == 0) return "0";
            if (limitToKB)
                return byteCount < 1024 ? $"{byteCount:n0} B" : $"{byteCount / 1024:n0} KB";
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = bytes / Math.Pow(1024, place);
            return $"{Math.Sign(byteCount) * num:0.00}" + " " + suf[place];
        }

        public static long GetSizeFromString(string sizeString) {
            var split = sizeString.Split();
            if (split.Length < 2) return 0;
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
           var size = (long)decimal.Parse(split[0].Replace(",", ""));
            var sizeSuffix = split[1];
            if (sizeSuffix == "B")
                return size;
            return size * (long)Math.Pow(1024, Array.IndexOf(suf, sizeSuffix));
        }

        public static long GetTotalBytes(FileList files)
        {
            return files.Count > 0 ? files.GetList().Sum(f => new FileInfo(f.GetFilePath()).Length) : 0;
        }

        public static int Clamp(int val, int min, int max)
        {
            if (val.CompareTo(min) < 0) return min;
            return val.CompareTo(max) > 0 ? max : val;
        }

        public static bool CheckSecurePassword(string pass)
        {
            if (pass.Length < Cipher.MINIMUM_PLAINTEXT_KEY_LENGTH)
                return false;
            if (!pass.Any(Char.IsUpper))
                return false;
            if (!Regex.IsMatch(pass, $"[{String.Join("", Cipher.ALLOWED_PLAINTEXT_KEY_SYMBOLS)}]+"))
                return false;
            return true;
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

        public static void InsertAndTrim<T>(this List<T> list, T item, int capacity)
        {
            list.Insert(0, item);
            if (list.Count > capacity)
                list.RemoveRange(capacity, list.Count - capacity);
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
            settings.processInOrder = false;
            settings.processInOrderBy = 0;
            settings.processInOrderDesc = false;
            settings.confirmOnExit = true;
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
            var minTicks = DateTime.Parse("01/01/2000").ToFileTime();
            var maxTicks = DateTime.Parse("12/31/2099").ToFileTime();
            return DateTime.FromFileTime(randomTicks(Math.Max(minTicks, DateTime.MinValue.Ticks), Math.Min(maxTicks, DateTime.MaxValue.Ticks)));
        }

        private static long randomTicks(long min, long max)
        {
            var buffer = new byte[8];
            new Random().NextBytes(buffer);
            long longRand = BitConverter.ToInt64(buffer, 0);
            return Math.Abs(longRand % (max - min)) + min;
        }

        public static void StandyComputer()
        {
            Application.SetSuspendState(PowerState.Suspend, true, false);
        }

        public static void RestartComputer()
        {
            var psi = new ProcessStartInfo("shutdown", "/r /t 0") {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi);
        }

        public static void ShutdownComputer()
        {
            var psi = new ProcessStartInfo("shutdown", "/s /t 0") {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi);
        }
    }
}