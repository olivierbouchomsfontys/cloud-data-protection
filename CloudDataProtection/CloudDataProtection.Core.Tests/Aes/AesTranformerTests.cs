using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using Microsoft.Extensions.Options;
using Xunit;

namespace CloudDataProtection.Core.Tests.Aes
{
    public class AesTranformerTests
    {
        private readonly ITransformer _transformer;

        public AesTranformerTests()
        {
            _transformer = new AesTransformer(Options.Create(new AesOptions
            {
                KeySize = 256,
                BlockSize = 128,
                EncryptionKey = "KD6Gg7K9jdxYNMvRIs4fh21DwgaDCcNAFGBKBn4G9xE=",
                EncryptionIv = "pdfn2d+30CvymJp2nAp1fg=="
            }));
        }

        [Fact]
        public void IntegrationTest()
        {
            string value = "cool value";

            string encrypted = _transformer.Encrypt(value);
            string decrypted = _transformer.Decrypt(encrypted);

            Assert.NotEqual(value, encrypted);
            Assert.Equal(value, decrypted);
        }
        
        [Fact]
        public void IntegrationTestLongData()
        {
            string value = "verylongrandomstring1234567890qwertyuiopasdfghjklzxcvbnm";

            string encrypted = _transformer.Encrypt(value);
            string decrypted = _transformer.Decrypt(encrypted);

            Assert.NotEqual(value, encrypted);
            Assert.Equal(value, decrypted);
        }
    }
}