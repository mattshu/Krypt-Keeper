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
        /*private static BackgroundWorker _worker1; // TODO #SplitLargeFileProject
        private static BackgroundWorker _worker2;
        private static BackgroundWorker _worker3;
        private static readonly BackgroundWorker _worker4;
        private static readonly BackgroundWorker[] _workers;

        private static Stream _rStream;
        private static Stream _cStream;*/

        public static readonly char[] ALLOWED_PLAINTEXT_KEY_SYMBOLS =
            {'!', '@', '#', '$', '%', '^', '&', '*', '?', '_', '~', '-', '£', '(', ')'};

        private static bool _cancelProcessing;
        private static BackgroundWorker _backgroundWorker;
        private static readonly Status _status = Status.GetInstance();

        private static long _currentPayloadState;
        private static long _currentPayloadTotal;
        private static long _totalPayloadState;
        private static long _totalPayloadTotal;
        private static int _totalFilesState;
        private static int _totalFilesTotal;

        private static long _lastDataSizeWorked;

        public static void CancelProcessing() => _cancelProcessing = true;

        public static string GetFileProgress() => $"{_totalFilesState}/{_totalFilesTotal} files processed";

        public static long GetPayloadState() => _totalPayloadState;
        public static long GetTotalSize() => _totalPayloadTotal;

        /*static Cipher() {// TODO #SplitLargeFileProject
            _worker1 = new BackgroundWorker();
            _worker1.DoWork += worker1_DoWork;
            _worker1.RunWorkerCompleted += worker1_RunWorkerCompleted;
            _worker2 = new BackgroundWorker();
            _worker2.DoWork += worker2_DoWork;
            _worker2.RunWorkerCompleted += worker2_RunWorkerCompleted;
            _worker3 = new BackgroundWorker();
            _worker3.DoWork += worker3_DoWork;
            _worker3.RunWorkerCompleted += worker3_RunWorkerCompleted;
            _worker4 = new BackgroundWorker();
            _worker4.DoWork += worker4_DoWork;
            _worker4.RunWorkerCompleted += worker4_RunWorkerCompleted;
        }

        private static void worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var data = e.Result;
            checkIfAllWorkersDone(0, data);
        }

        private static void worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var data = e.Result;
            checkIfAllWorkersDone(1, data);
        }

        private static void worker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var data = e.Result;
            checkIfAllWorkersDone(2, data);
        }

        private static void worker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var data = e.Result;
            checkIfAllWorkersDone(3, data);
        }

        private static bool worker1Done;
        private static bool worker2Done;
        private static bool worker3Done;
        private static bool worker4Done;

        private static List<object> dataList = new List<object>();

        private static void checkIfAllWorkersDone(int worker, object data)
        {
            dataList.Insert(worker, data);

            switch (worker)
            {
                case 0:
                    worker1Done = true;
                    break;
                case 1:
                    worker2Done = true;
                    break;
                case 2:
                    worker3Done = true;
                    break;
                case 3:
                    worker4Done = true;
                    break;
            }

            if (!worker1Done || !worker2Done || !worker3Done || !worker4Done) return;
            Console.WriteLine(@"Data finished! Count: " + dataList.Count);
        }
        */

        public static void ProcessFiles(CipherOptions options)
        {
            if (options.Files.Count <= 0) return;
            _totalFilesState = 0;
            if (options.Mode == Mode.Decrypt)
                validateFilesForDecryption(options);
            _totalFilesTotal = options.Files.Count;
            _totalPayloadTotal = Utils.GetTotalBytes(options.Files);
            _status.StartProcessRateCollection(options);
            _backgroundWorker.RunWorkerAsync(options);
        }

        private static void validateFilesForDecryption(CipherOptions options)
        {
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

        public static long GetElapsedBytes()
        {
            if (_lastDataSizeWorked == 0)
            {
                _lastDataSizeWorked = _totalPayloadState;
                return 0;
            }
            var difference = Math.Abs(_totalPayloadState - _lastDataSizeWorked);
            _lastDataSizeWorked = _totalPayloadState;
            return difference;
        }

        public static void SetBackgroundWorker(BackgroundWorker bgWorker)
        {
            _backgroundWorker = bgWorker;
            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = (CipherOptions) e.Argument;
            foreach (var fileData in options.Files.GetList().ToList())
            {
                if (_backgroundWorker.CancellationPending)
                    break;
                if (File.Exists(fileData.GetFilePath()))
                {
                    _status.WritePending($"{options.GetModeOfOperation()}: " + fileData.GetFilePath());
                    _status.UpdateOperationStatus(options.GetModeOfOperation());
                    processFile(fileData.GetFilePath(), options);
                    _totalFilesState++;
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
            _currentPayloadState = 0;
            _currentPayloadTotal = 0;
            _totalPayloadState = 0;
            _totalPayloadTotal = 0;
            _totalFilesState = 0;
            _totalFilesTotal = 0;
            _lastDataSizeWorked = 0;
        }

        private static void processFile(string path, CipherOptions options)
        {
            string workingPath = "";
            try
            {
                if (!File.Exists(path))
                {
                    _status.WriteLine("* File not found: " + path);
                    return;
                }
                _status.UpdateProcessedFile(Path.GetFileName(path));
                if (options.Mode == Mode.Decrypt && isFileValidForDecryption(path))
                    workingPath = path.ReplaceLastOccurrence(FILE_EXTENSION, WORKING_FILE_EXTENSION);
                else if (options.Mode == Mode.Encrypt)
                    workingPath = getEncryptionWorkingPath(path, options);
                else return;
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
                if (_cancelProcessing)
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

        private static bool isFileValidForDecryption(string path)
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

        /*private static void worker1_DoWork(object sender, DoWorkEventArgs e) // TODO #SplitLargeFileProject
        {
            taskWorker((BackgroundWorkerArguments)e.Argument);
        }

        private static void worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            taskWorker((BackgroundWorkerArguments)e.Argument);
        }

        private static void worker3_DoWork(object sender, DoWorkEventArgs e)
        {
            taskWorker((BackgroundWorkerArguments)e.Argument);
        }

        private static void worker4_DoWork(object sender, DoWorkEventArgs e)
        {
            taskWorker((BackgroundWorkerArguments)e.Argument);
        }

        private static void taskWorker(BackgroundWorkerArguments args)
        {
            var path = args.Path;
            var offset = args.Offset;
            var size = new FileInfo(path).Length;
            using (var rStream = args.ReadStream)
            {
                using (var cStream = args.CryptoStream)
                {
                    rStream.Seek(offset, SeekOrigin.Begin);
                    _cancelProcessing = false;
                    var buffer = new byte[CHUNK_SIZE];
                    int bytesRead = rStream.Read(buffer, 0, CHUNK_SIZE);
                    //_currentPayloadState = 0;
                    //_currentPayloadTotal = new FileInfo(path).Length;
                    int totalRead = 0;
                    while (offset > size ? bytesRead > 0 : Math.Ceiling(size / 4D) >= totalRead || bytesRead != 0)
                    {
                        // todo I don't trust this
                        if (_cancelProcessing)
                            break;
                        //_currentPayloadState += bytesRead;
                        //_totalPayloadState += bytesRead;
                        //var packet = buildProgressPacket();
                        //_backgroundWorker.ReportProgress(0, packet);
                        cStream.Write(buffer, 0, bytesRead);
                        bytesRead = rStream.Read(buffer, 0, bytesRead);
                        totalRead += bytesRead;
                    }
                }
            }
        }

        private class BackgroundWorkerArguments {
            public string Path { get; }
            public long Offset { get; }
            public Stream ReadStream { get; }
            public Stream CryptoStream { get; }

            public BackgroundWorkerArguments(string path, long offset, Stream readStream, Stream cryptoStream) {
                Path = path;
                Offset = offset;
                ReadStream = readStream;
                CryptoStream = cryptoStream;
            }
        }*/

        private static void processStreams(string path, Stream readStream, Stream cryptoStream)
        {
            /* //TODO #SplitLargeFileProject
                13413418 / 4 	= 3353354.5
	                 ^ ceil	= 3353355 = a

                Worker1: 0, 3353354 (a * 0, a * 1 - 1)
                Worker2: 3353355, 6706709 (a * 1, a * 2 - 1)
                Worker3: 6706710, 10060064 (a * 2, a * 3 - 1)
                Worker4: 10060065, 13413420 (a * 3, a * 4 - 1)

                Worker4.(while read != 0)

            var size = new FileInfo(path).Length;
            if (size > Utils.GetSizeFromString("64 MB"))
            {
                var offset = (long) Math.Ceiling((double)size / 4);
                for (int i = 0; i < 4; i++)
                {
                    // Assign each background worker the task of decoding a quarter of the file each, going by the offset below
                    var args = new BackgroundWorkerArguments(path, offset * (i + 1) - 1, readStream, cryptoStream);
                    switch (i)
                    {
                        case 0:
                            _worker1.RunWorkerAsync(args);
                            break;
                        case 1:
                            _worker2.RunWorkerAsync(args);
                            break;
                        case 2:
                            _worker3.RunWorkerAsync(args);
                            break;
                        case 3:
                            _worker4.RunWorkerAsync(args);
                            break;
                    }
                }
            }
            else
            {
                var args = new BackgroundWorkerArguments(path, 0, readStream, cryptoStream);
                _worker1.RunWorkerAsync(args);
            }
            */
            _cancelProcessing = false;
            var buffer = new byte[CHUNK_SIZE];
             int bytesRead = readStream.Read(buffer, 0, CHUNK_SIZE);
            _currentPayloadState = 0;
            _currentPayloadTotal = new FileInfo(path).Length;
            while (bytesRead > 0)
            {
                if (_cancelProcessing)
                    break;
                _currentPayloadState += bytesRead;
                _totalPayloadState += bytesRead;
                var packet = buildProgressPacket();
                _backgroundWorker.ReportProgress(0, packet);
                cryptoStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, bytesRead);
            }
        }

        private static ProgressPacket buildProgressPacket()
        {
            return new ProgressPacket(
                new[] { _currentPayloadState, _currentPayloadTotal},
                new[] {_totalFilesState, _totalFilesTotal},
                new[] {_totalPayloadState, _totalPayloadTotal});
        }

        private static void buildAndWriteFooter(string path, Stream cryptoStream)
        {
            var footer = new Footer();
            footer.Build(path);
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
            using (var fOpen = new FileStream(workingPath, FileMode.Open))
                fOpen.SetLength(fOpen.Length - footer.ToArray().Length);
            var originalPath = workingPath.Replace(Path.GetFileName(workingPath), footer.Name)
                .ReplaceLastOccurrence(WORKING_FILE_EXTENSION, "");
            if (File.Exists(originalPath))
                originalPath = Utils.PadExistingFileName(originalPath);
            File.Move(workingPath, originalPath); // Copy worked file to original file
            Utils.SetFileTimesFromFooter(originalPath, footer);
            if (new FileInfo(originalPath).Length > 0)
            {
                if (options.RemoveOriginalDecryption && !Utils.TryDeleteFile(path))
                    _status.WriteLine("Unable to remove original file: " + path);
            }
            else
            {
                _status.WriteLine("* Error decrypting file: " + path);
                _status.WriteLine("* File possibly corrupt: " + originalPath);
                return string.Empty;
            }
            if (!Utils.TryDeleteFile(workingPath))
                _status.WriteLine("Unable to remove temp file: " + workingPath);
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
 