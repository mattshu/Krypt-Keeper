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
                                initializeEncryption(path, rStream, cStream);
                            }
                        }
                    }
                }
                postEncryptionFileHandling(path, options);
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

        private static void initializeEncryption(string path, Stream readStream, Stream cryptoStream)
        {
            var buffer = new byte[CHUNK_SIZE];
            readStream.Seek(0, SeekOrigin.Begin);
            int bytesRead = readStream.Read(buffer, 0, CHUNK_SIZE);
            while (bytesRead > 0)
            {
                if (bytesRead < CHUNK_SIZE)
                    Array.Resize(ref buffer, bytesRead);
                cryptoStream.Write(buffer, 0, buffer.Length);
                bytesRead = readStream.Read(buffer, 0, bytesRead);
            }

            var footer = new Footer();
            footer.Build(path);
            var footerData = footer.ToArray();
            cryptoStream.Write(footerData, 0, footerData.Length);
        }

        private static void postEncryptionFileHandling(string path, CipherOptions options)
        {
            if (options.RemoveOriginal)
                File.Delete(path);
            if (options.MaskFileName)
                File.Move(path + FILE_EXTENSION,
                    path.Replace(Path.GetFileName(path), Helper.GetRandomAlphanumericString(16)) + FILE_EXTENSION);
        }

        private static void decrypt(string path, CipherOptions options)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            status.WritePending("Decrypting: " + path);
            using (var provider = Helper.GetAlgorithm(options.Mode))
            {
                provider.Key = options.Key;
                provider.IV = getIV(path);
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                var encryptor = provider.CreateDecryptor(provider.Key, provider.IV);
                var decryptedPath = path.Replace(FILE_EXTENSION, "");
                var footer = new Footer();
                decryptedPath = renameFileWithPaddingIfExists(decryptedPath);
                using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var wStream = new FileStream(decryptedPath, FileMode.CreateNew, FileAccess.Write))
                    {
                        using (var cStream = new CryptoStream(wStream, encryptor, CryptoStreamMode.Write))
                        {
                            initializeDecryption(rStream, cStream);
                        }
                    }
                }
                File.Delete(path);
                postDecryptionFileHandling(decryptedPath, footer);
            }
        }

        private static void initializeDecryption(Stream readStream, Stream cryptoStream)
        {
            var buffer = new byte[CHUNK_SIZE];
            readStream.Seek(16, SeekOrigin.Begin); // 16 to skip IV
            int bytesRead = readStream.Read(buffer, 0, CHUNK_SIZE);
            while (bytesRead > 0)
            {
                if (bytesRead < CHUNK_SIZE)
                    Array.Resize(ref buffer, bytesRead);
                cryptoStream.Write(buffer, 0, buffer.Length);
                bytesRead = readStream.Read(buffer, 0, bytesRead);
            }
        }

        private static void postDecryptionFileHandling(string path, Footer footer)
        {
            if (!footer.TryExtract(path))
                throw new Exception("Unable to generate file information from " + path);
            using (var fOpen = new FileStream(path, FileMode.Open))
                fOpen.SetLength(fOpen.Length - footer.ToArray().Length);
            var newPath = path.Replace(Path.GetFileName(path), footer.Name); // Path with name of original file
            newPath = renameFileWithPaddingIfExists(newPath);
            File.Move(path, newPath);
            File.Delete(path);
            Helper.SetFileTimes(newPath, footer); // Set to original filetimes
        }

        private static string renameFileWithPaddingIfExists(string path)
        {
            if (!File.Exists(path)) return path;
            string newPath;
            do
            {
                newPath = addRandomPaddingToFileName(path);
            } while (File.Exists(newPath));
            status.WriteLine("*** File already exists (" + path + ") Renaming to: " + newPath);
            path = newPath;
            return path;
        }

        private static string addRandomPaddingToFileName(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            return path.Replace(fileName ?? "", fileName + Helper.GetRandomAlphanumericString(5));
        }

        private static byte[] getIV(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (var bReader = new BinaryReader(fStream))
                    return bReader.ReadBytes(16);
        }
    }
}