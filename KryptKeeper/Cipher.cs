using System;
using System.IO;
using System.Security.Cryptography;

namespace KryptKeeper
{
    internal class Cipher
    {
        private static readonly Status status = Status.GetInstance();

        public const string FILE_EXTENSION = ".krpt";
        private const int CHUNK_SIZE = 768 * 1024 * 1024; // 768MB

        public static void EncryptFiles(string[] files, CipherOptions options)
        {
            if (files.Length <= 0) return;
            foreach (var file in files)
                encrypt(file, options);
            status.WriteLine("Encryption completed.");
        }

        public static void DecryptFiles(string[] files, CipherOptions options)
        {
            if (files.Length <= 0) return;
            foreach (var file in files)
                decrypt(file, options);
            status.WriteLine("Decryption completed.");
        }

        private static void encrypt(string path, CipherOptions options)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException(path);
                status.WritePending("Encrypting: " + path);

                using (var provider = Helper.GetAlgorithm(options.Mode))
                {
                    provider.Key = options.Key;
                    provider.IV = options.IV;
                    provider.Mode = CipherMode.CBC;
                    provider.Padding = PaddingMode.PKCS7;
                    var encryptor = provider.CreateEncryptor(provider.Key, provider.IV);
                    using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (var wStream = new FileStream(path + FILE_EXTENSION, FileMode.CreateNew, FileAccess.Write))
                        {
                            wStream.Write(options.IV, 0, options.IV.Length);
                            using (var cStream = new CryptoStream(wStream, encryptor, CryptoStreamMode.Write))
                            {
                                var buffer = new byte[CHUNK_SIZE];
                                rStream.Seek(0, SeekOrigin.Begin);
                                int bytesRead = rStream.Read(buffer, 0, CHUNK_SIZE);
                                while (bytesRead > 0)
                                {
                                    cStream.Write(buffer, 0, bytesRead);
                                    bytesRead = rStream.Read(buffer, 0, bytesRead);
                                }
                                var footer = new Footer();
                                footer.Build(path);
                                var footerData = footer.ToArray();
                                cStream.Write(footerData, 0, footerData.Length);
                            }
                        }
                    }
                }
                
                if (options.RemoveOriginal)
                    File.Delete(path);
                if (options.MaskFileName)
                    File.Move(path + FILE_EXTENSION,
                        path.Replace(Path.GetFileName(path), Helper.GetRandomAlphanumericString(16)) + FILE_EXTENSION);
            }
            catch (FileNotFoundException ex)
            {
                status.WriteLine("*** Error: File not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                status.WriteLine("*** Error occured: " + ex.StackTrace + ex.Message);
            }
        }

        private static void decrypt(string path, CipherOptions options)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            status.WritePending("Decrypting: " + path);
            using (var provider = Helper.GetAlgorithm(options.Mode))
            {
                provider.Key = options.Key;
                provider.IV = extractIV(path);
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                var encryptor = provider.CreateDecryptor(provider.Key, provider.IV);
                var decryptedPath = path.Replace(FILE_EXTENSION, "");
                var footer = new Footer();
                using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (File.Exists(decryptedPath))
                        decryptedPath += Helper.GetRandomAlphanumericString(3);
                    using (var wStream = new FileStream(decryptedPath, FileMode.CreateNew, FileAccess.Write))
                    {
                        using (var cStream = new CryptoStream(wStream, encryptor, CryptoStreamMode.Write))
                        {
                            var buffer = new byte[CHUNK_SIZE];
                            rStream.Seek(16, SeekOrigin.Begin); // 16 to skip IV
                            int bytesRead = rStream.Read(buffer, 0, CHUNK_SIZE);
                            while (bytesRead > 0)
                            {
                                if (bytesRead < CHUNK_SIZE)
                                    Array.Resize(ref buffer, bytesRead);
                                cStream.Write(buffer, 0, buffer.Length);
                                bytesRead = rStream.Read(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
                if (!footer.TryExtract(decryptedPath))
                    throw new Exception("Unable to generate file information from " + decryptedPath);
                using (var fOpen = new FileStream(decryptedPath, FileMode.Open))
                {
                    fOpen.SetLength(fOpen.Length - footer.ToArray().Length);
                }
                var newOriginalPath = decryptedPath.Replace(Path.GetFileName(decryptedPath), footer.Name); // Path with name of original file
                bool decryptedAlreadyExists = File.Exists(newOriginalPath);
                if (decryptedAlreadyExists)
                    newOriginalPath = renameFileWithPadding(newOriginalPath);
                File.Move(decryptedPath, newOriginalPath);
                File.Delete(decryptedPath);
                File.Delete(path);
                Helper.SetFileTimes(newOriginalPath, footer); // Set to original filetimes
            }
        }

        private static string renameFileWithPadding(string newOriginalPath)
        {
            string newPath;
            do
            {
                newPath = addRandomPaddingToFileName(newOriginalPath);
            } while (File.Exists(newPath));
            status.WriteLine("*** File already exists (" + newOriginalPath + ") Renaming to: " + newPath);
            newOriginalPath = newPath;
            return newOriginalPath;
        }

        private static string addRandomPaddingToFileName(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            return path.Replace(fileName ?? "", fileName + Helper.GetRandomAlphanumericString(5));
        }

        private static byte[] extractIV(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var bReader = new BinaryReader(fStream))
                {
                    return bReader.ReadBytes(16);
                }
            }
        }
    }
}