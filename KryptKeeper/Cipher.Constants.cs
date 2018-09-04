namespace KryptKeeper
{
    internal static partial class Cipher
    {
        public const string FILE_EXTENSION = ".krpt";
        public const string WORKING_FILE_EXTENSION = ".krpt.tmp";
        public const long MAX_FILE_LENGTH = 0x800000000; // 4GB
        public const int MINIMUM_PLAINTEXT_KEY_LENGTH = 8;
        public const int MINIMUM_FILE_LENGTH = IV_SIZE + SALT_SIZE + ENTROPY_SIZE;
        public const int IV_SIZE = 16;
        public const int SALT_SIZE = 29;
        public const int ENTROPY_SIZE = 15;
        public const int KEY_SIZE = 256;
        public const int CHUNK_SIZE = 0x1000000; // 16MB 
    }
}