using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KryptKeeper
{
    public static partial class Cipher
    {
        public static void CancelProcessing() => _CancelProcessing = true;
        public static string GetFileProgress() => $"{_TotalFilesState}/{_TotalFilesTotal} files processed";
        public static long GetPayloadState() => _TotalPayloadState;
        public static long GetTotalSize() => _TotalPayloadTotal;
        private static bool _CancelProcessing;
        private static BackgroundWorker _BackgroundWorker;
        private static readonly Status _status = Status.GetInstance();
        private static long _CurrentPayloadState;
        private static long _CurrentPayloadTotal;
        private static long _TotalPayloadState;
        private static long _TotalPayloadTotal;
        private static int _TotalFilesState;
        private static int _TotalFilesTotal;
        private static long _LastDataSizeWorked;
        
        public static void ProcessFiles(CipherOptions options)
        {
            if (options.Files.Count <= 0) return;
            _TotalFilesState = 0;
            if (options.Mode == Mode.Decrypt)
                validateFilesForDecryption(options);
            _TotalFilesTotal = options.Files.Count;
            _TotalPayloadTotal = Utils.GetTotalBytes(options.Files);
            _status.StartCollection(options);
            _BackgroundWorker.RunWorkerAsync(options);
        }

        private static void validateFilesForDecryption(CipherOptions options) {
            var tmpList = options.Files.GetList()
                .Where(x => !x.GetExtension().Equals(FILE_EXTENSION))
                .Select(x => x).ToList();
            if (tmpList.Count <= 0) return;
            var validList = options.Files.GetList()
                .Where(x => x.GetExtension().Equals(FILE_EXTENSION))
                .Select(x => x).ToList();
            options.Files.SetList(validList);
            _status.WriteLine("The following file(s) were not valid for decryption:");
            foreach (var file in tmpList)
                _status.WriteLine("-" + file.GetFilePath());
        }

        public static long GetElapsedBytes() {
            if (_LastDataSizeWorked == 0) {
                _LastDataSizeWorked = _TotalPayloadState;
                return 0;
            }
            var difference = Math.Abs(_TotalPayloadState - _LastDataSizeWorked);
            _LastDataSizeWorked = _TotalPayloadState;
            return difference;
        }

        public static void SetBackgroundWorker(BackgroundWorker bgWorker) {
            _BackgroundWorker = bgWorker;
            _BackgroundWorker.DoWork += backgroundWorker_DoWork;
            _BackgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = (CipherOptions) e.Argument;
            foreach (var fileData in options.Files.GetList().ToList())
            {
                if (_BackgroundWorker.CancellationPending)
                    break;
                if (File.Exists(fileData.GetFilePath()))
                {
                    _status.WritePending($"{options.GetModeOfOperation()}: " + fileData.GetFilePath());
                    _status.SetOperationText(options.GetModeOfOperation());
                    processFile(fileData.GetFilePath(), options);
                    _TotalFilesState++;
                }
                else
                    _status.WriteLine("* Unable to find: " + fileData);
            }
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            resetProgressPoints();
        }

        private static void resetProgressPoints()
        {
            _CurrentPayloadState = 0;
            _CurrentPayloadTotal = 0;
            _TotalPayloadState = 0;
            _TotalPayloadTotal = 0;
            _TotalFilesState = 0;
            _TotalFilesTotal = 0;
            _LastDataSizeWorked = 0;
        }

        private static void processFile(string path, CipherOptions options)
        {
            string workingPath = "";
            try
            {
                if (!File.Exists(path)) {
                    _status.WriteLine("* File not found: " + path);
                    return;
                }
                _status.SetFileWorkedText(Path.GetFileName(path));
                switch (options.Mode) {
                    case Mode.Decrypt when fileIsValidForDecryption(path):
                        workingPath = path.ReplaceLastOccurrence(FILE_EXTENSION, WORKING_FILE_EXTENSION);
                        break;
                    case Mode.Encrypt:
                        workingPath = getEncryptionWorkingPath(path, options);
                        break;
                    default:
                        return;
                }
                if (File.Exists(workingPath)) workingPath = Utils.PadExistingFileName(workingPath);
                using (var aes = Aes.Create())
                {
                    if (aes == null)
                    {
                        _status.WriteLine("*** Error: Failed to create AES object!");
                        return;
                    }
                    var transformer = createCryptoTransform(path, options, aes);
                    using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (var wStream = new FileStream(workingPath, FileMode.CreateNew, FileAccess.Write))
                        {
                            if (options.Mode == Mode.Encrypt)
                                injectHeader(options, wStream);
                            using (var cStream = new CryptoStream(wStream, transformer, CryptoStreamMode.Write))
                            {
                                rStream.Seek(options.Mode == Mode.Encrypt ? 0 : ENTROPY_SIZE + IV_SIZE + SALT_SIZE, SeekOrigin.Begin);
                                processStreams(path, rStream, cStream);
                                if (options.Mode == Mode.Encrypt)
                                    buildAndWriteFooter(path, cStream);
                            }
                        }
                    }
                }
                if (_CancelProcessing)
                {
                    _status.WriteLine("Process cancelled, deleting temp file: " + workingPath);
                    if (!Utils.TryDeleteFile(workingPath))
                        _status.WriteLine("Unable to remove temp file: " + workingPath);
                }
                else
                {
                    var resultFile = postProcessFileHandling(path, workingPath, options);
                    if (resultFile.Length > 0)
                        _status.WriteLine("Processed to: " + Path.GetFileName(resultFile));
                }
            }
            catch (Exception ex)
            {
                handleCipherExceptions(ex, path, workingPath);
            }
        }

        private static string getEncryptionWorkingPath(string path, CipherOptions options)
        {
            string workingPath;
            if (options.MaskFileName)
            {
                workingPath = path.Replace(Path.GetFileName(path), Utils.GetRandomAlphanumericString(16)) + FILE_EXTENSION;
            }
            else
            {
                if (Path.GetExtension(path) == FILE_EXTENSION)
                    workingPath = Utils.PadExistingFileName(path);
                else
                    workingPath = path + FILE_EXTENSION;
            }
            return workingPath;
        }

        private static bool fileIsValidForDecryption(string path)
        {
            if (Path.GetExtension(path) == FILE_EXTENSION && new FileInfo(path).Length >= MINIMUM_FILE_LENGTH)
                return true;
            _status.WriteLine("* File not in correct format for decryption: " + path);
            return false;
        }

        private static ICryptoTransform createCryptoTransform(string path, CipherOptions options, SymmetricAlgorithm aes)
        {
            aes.KeySize = KEY_SIZE;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform transformer;
            if (options.Mode == Mode.Encrypt)
            {
                aes.Key = generateSaltedKey(options.Key, options.Salt);
                aes.IV = options.IV;
                transformer = aes.CreateEncryptor(aes.Key, aes.IV);
            }
            else
            {
                aes.Key = generateSaltedKey(options.Key, extractSalt(path));
                aes.IV = extractIV(path);
                transformer = aes.CreateDecryptor(aes.Key, aes.IV);
            }
            return transformer;
        }

        private static byte[] generateSaltedKey(byte[] key, byte[] salt)
        {
            var saltedKey = BCrypt.HashPassword(Encoding.UTF8.GetString(key), Encoding.UTF8.GetString(salt));
            return Utils.GetSHA256(Utils.GetBytes(saltedKey));
        }

        private static byte[] extractIV(string path)
        {
            try
            {
                using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var bReader = new BinaryReader(fStream))
                    {
                        fStream.Seek(ENTROPY_SIZE, SeekOrigin.Begin);
                        var read = bReader.ReadBytes(IV_SIZE);
                        if (read.Length > 0)
                            return read;
                        throw new CryptographicException("There was a problem extracting the IV from: " + path);
                    }
                }
            }
            catch (Exception ex)
            {
                _status.WriteLine("There was a problem extracting the IV from: " + path);
                _status.WriteLine("* Error: " + ex.Message);
                return new byte[0];
            }
        }

        private static byte[] extractSalt(string path)
        {
            try
            {
                using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var bReader = new BinaryReader(fStream))
                    {
                        fStream.Seek(IV_SIZE + ENTROPY_SIZE, SeekOrigin.Begin);
                        var read = bReader.ReadBytes(SALT_SIZE);
                        if (read.Length > 0)
                            return read;
                        throw new CryptographicException("There was a problem extracting the salt from: " + path);
                    }
                }
            }
            catch (Exception ex)
            {
                _status.WriteLine("There was a problem extracting the salt from: " + path);
                _status.WriteLine("* Error: " + ex.Message);
                return new byte[0];
            }
        }

        private static void injectHeader(CipherOptions options, Stream stream)
        {
            stream.Write(options.Entropy, 0, options.Entropy.Length); // Insert random entropy
            stream.Write(options.IV, 0, options.IV.Length); // Insert IV
            stream.Write(options.Salt, 0, options.Salt.Length); // Insert Salt
        }

        private static void processStreams(string path, Stream readStream, Stream cryptoStream)
        {
            _CancelProcessing = false;
            var buffer = new byte[CHUNK_SIZE];
             int bytesRead = readStream.Read(buffer, 0, CHUNK_SIZE);
            _CurrentPayloadState = 0;
            _CurrentPayloadTotal = new FileInfo(path).Length;
            while (bytesRead > 0)
            {
                if (_CancelProcessing)
                    break;
                _CurrentPayloadState += bytesRead;
                _TotalPayloadState += bytesRead;
                var packet = buildProgressPacket();
                _BackgroundWorker.ReportProgress(0, packet);
                cryptoStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, bytesRead);
            }
        }

        private static ProgressPacket buildProgressPacket()
        {
            return new ProgressPacket(
                new[] { _CurrentPayloadState, _CurrentPayloadTotal},
                new[] {_TotalFilesState, _TotalFilesTotal},
                new[] {_TotalPayloadState, _TotalPayloadTotal});
        }

        private static void buildAndWriteFooter(string path, Stream cryptoStream)
        {
            var footer = new Footer();
            footer.Build(path, MainWindow.Version);
            var footerData = footer.ToArray();
            cryptoStream.Write(footerData, 0, footerData.Length);
        }

        private static string postProcessFileHandling(string path, string workingPath, CipherOptions options)
        {
            return options.Mode == Mode.Encrypt ? encryptionPostProcess(path, workingPath, options) : decryptionPostProcess(path, workingPath, options);
        }

        private static string encryptionPostProcess(string path, string workingPath, CipherOptions options)
        {
            if (options.RemoveOriginalEncryption && !Utils.TryDeleteFile(path))
                _status.WriteLine("Unable to remove original (access denied): " + path);
            if (options.MaskFileDate)
                Utils.SetRandomFileTimes(workingPath);
            return workingPath;
        }

        private static string decryptionPostProcess(string path, string workingPath, CipherOptions options)
        {
            var footer = new Footer();
            if (!footer.TryExtract(workingPath))
            {
                _status.WriteLine("*** Error: Unable to get decrypted file information from " + path);
                _status.WriteLine("* File possibly corrupt: " + workingPath);
                return string.Empty;
            }
            var originalPath = workingPath.Replace(Path.GetFileName(workingPath), footer.Name)
                .ReplaceLastOccurrence(WORKING_FILE_EXTENSION, "");
            if (File.Exists(originalPath))
                originalPath = Utils.PadExistingFileName(originalPath);
            File.Move(workingPath, originalPath); // Copy worked file to original file
            if (new FileInfo(originalPath).Length > 0)
            {
                Utils.SetFileTimesFromFooter(originalPath, footer);
                if (options.RemoveOriginalDecryption && !Utils.TryDeleteFile(path))
                    _status.WriteLine("Unable to remove original file: " + path);
            }
            else
            {
                _status.WriteLine("* Error decrypting file: " + path);
                _status.WriteLine("* File possibly corrupt: " + originalPath);
                return string.Empty;
            }
            return originalPath;
        }

        private static void handleCipherExceptions(Exception ex, string path, string workingPath)
        {
            var baseExceptionRaw = ex.GetBaseException().ToString();
            var baseExceptionPart = baseExceptionRaw.Substring(0, baseExceptionRaw.IndexOf(':'));
            var baseException = baseExceptionPart.Substring(baseExceptionPart.LastIndexOf('.') + 1);
            var msgWhenUnhandled = ex.Message + Environment.NewLine + ex.StackTrace;
            var preserveTempFile = false;
            switch (baseException)
            {
                case "CryptographicException":
                    if (ex.Message.Equals("The input data is not a complete block.") ||
                        ex.Message.Equals("Padding is invalid and cannot be removed."))
                        _status.WriteLine("*** Failed to decrypt file: " + path);
                    else
                        _status.WriteLine("*** Cryptographic exception: " + msgWhenUnhandled);
                    break;

                case "ArgumentException":
                    _status.WriteLine("*** Failed to decrypt file: " + path);
                    break;

                case "UnauthorizedAccessException":
                    _status.WriteLine("*** Unauthorized access: " + ex.Message);
                    break;

                default:
                    _status.WriteLine("*** UNHANDLED EXCEPTION: " + msgWhenUnhandled);
                    _status.WriteLine("Partially processed file will be preserved: " + workingPath);
                    preserveTempFile = true;
                    break;
            }
            if (!preserveTempFile && !Utils.TryDeleteFile(workingPath))
                _status.WriteLine("Unable to remove temp file: " + workingPath);
        }
    }
}
 