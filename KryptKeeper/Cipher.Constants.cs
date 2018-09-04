namespace KryptKeeper
{
    internal static partial class Cipher
    {
        public const string FILE_EXTENSION = ".krpt";
        public const string WORKING_FILE_EXTENSION = ".krpt.tmp";
        public const long MAX_FILE_LENGTH = 0x800000000; // 4GB
        private const int MINIMUM_FILE_LENGTH = IV_SIZE + SALT_SIZE + ENTROPY_SIZE;
        private const int IV_SIZE = 16;
        private const int SALT_SIZE = 29;
        private const int ENTROPY_SIZE = 15;
        private const int KEY_SIZE = 256;
        private const int CHUNK_SIZE = 0x1000000; // 16MB
    }
}