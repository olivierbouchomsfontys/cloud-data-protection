using System;
using System.IO;
using System.Text;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using Xunit;

namespace CloudDataProtection.Core.Tests.Aes
{
    public class AesStreamTransformerTests
    {
        private readonly IFileTransformer _transformer;

        public AesStreamTransformerTests()
        {
            _transformer = new AesStreamTransformer(new AesOptions
            {
                KeySize = 256,
                BlockSize = 128,
                EncryptionKey = "KD6Gg7K9jdxYNMvRIs4fh21DwgaDCcNAFGBKBn4G9xE=",
                EncryptionIv = "pdfn2d+30CvymJp2nAp1fg=="
            });
        }
        
        [Fact]
        public void TestEncrypt()
        {
            const string data = "cool value";

            byte[] bytes = Encoding.UTF8.GetBytes(data);

            byte[] encryptedBytes;

            // Encrypt
            using (MemoryStream input = new MemoryStream())
            {
                input.Write(bytes);
                input.Flush();

                using (MemoryStream encrypted = _transformer.Encrypt(input) as MemoryStream)
                {
                    encryptedBytes = encrypted.ToArray();

                    Assert.NotEqual(bytes, encryptedBytes);
                }
            }

            // Decrypt
            using (MemoryStream test = new MemoryStream())
            {
                test.Write(encryptedBytes);
                test.Flush();

                byte[] decrypted = _transformer.Decrypt(test);

                Assert.Equal(bytes, decrypted);
                Assert.Equal(data, Encoding.UTF8.GetString(decrypted));
            }
        }
    }
}