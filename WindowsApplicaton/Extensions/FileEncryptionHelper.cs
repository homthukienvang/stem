using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Extensions
{
    public static class FileEncryptionHelper
    {
        private static readonly MemoryStream MStream;

        static FileEncryptionHelper()
        {
            MStream = new MemoryStream();
        }

        public static void Decrypt(string file, string password)
        {
            //var fileName = Process.GetCurrentProcess().ProcessName;

            var key = new byte[32]; // 256 bits key
            Encoding.Default.GetBytes(password).CopyTo(key, 0); // padding with 0

            var aes = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256,
                Padding = PaddingMode.Zeros
            };

            using (var outputStream = new FileStream(file + Constants.FileExtension, FileMode.Create))
            {
                using (var cryptoStream = new CryptoStream(outputStream, aes.CreateDecryptor(key, key), CryptoStreamMode.Write))
                {
                    // reading the content of the current process
                    var buffer = File.ReadAllBytes(string.Format("{0}{1}", file, Constants.FileExtension));
                    // skip the original decryptor's size (we don't want to decrypt that!)
                    cryptoStream.Write(buffer, Constants.DecryptorSize, buffer.Length - Constants.DecryptorSize);
                }
            }
        }

        public static void Encrypt(string fileName, string password)
        {
            var key = new byte[32];  // same key (256 bits)
            Encoding.Default.GetBytes(password).CopyTo(key, 0);  // padding with 0 once again

            var aes = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256,
                Padding = PaddingMode.Zeros
            };

            using (var cStream = new CryptoStream(MStream, aes.CreateEncryptor(key, key), CryptoStreamMode.Write))
            {
                // reading the content of the file that requires password protection
                var buffer = File.ReadAllBytes(fileName);
                // encrypting & storing everything in a MemoryStream
                cStream.Write(buffer, 0, buffer.Length);
            }
            Append(fileName); // time to append
        }

        private static void Append(string file)
        {
            // reading the content of the original decryptor
            var exeBuffer = File.ReadAllBytes("buffer.bak");

            // extracting the encrypted content from the MemoryStream
            var appendBuffer = MStream.ToArray();

            // this buffer is the 'new' decryptor, that contains the new file
            var finalBuffer = new byte[exeBuffer.Length + appendBuffer.Length];

            exeBuffer.CopyTo(finalBuffer, 0);
            appendBuffer.CopyTo(finalBuffer, exeBuffer.Length);

            // creating 'SomeFile.txt.exe'
            File.WriteAllBytes(string.Format("{0}{1}", file, Constants.FileExtension), finalBuffer);
        }
    }
}
