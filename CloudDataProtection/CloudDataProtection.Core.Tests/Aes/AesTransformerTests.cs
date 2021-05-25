using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using Microsoft.Extensions.Options;
using Xunit;

namespace CloudDataProtection.Core.Tests.Aes
{
    public class AesTransformerTests
    {
        private readonly ITransformer _transformer;

        public AesTransformerTests()
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
        public void TestEncrypt()
        {
            const string value = "cool value";
            const string expected = "kghQugoltbEgR9jY0LkJjw==";

            string encrypted = _transformer.Encrypt(value);
            
            Assert.Equal(expected, encrypted);
        }

        [Fact]
        public void TestDecrypt()
        {
            const string value = "kghQugoltbEgR9jY0LkJjw==";
            const string expected = "cool value";
            
            string decrypted = _transformer.Decrypt(value);

            Assert.Equal(expected, decrypted);
        }
        
        [Fact]
        public void TestEncryptLongData()
        {
            const string value = "verylongrandomstring1234567890qwertyuiopasdfghjklzxcvbnm";
            const string expected = "GfbFiLjsYPaMNrhvWWHpl2xbUSD9Amr0FGTnX7y9DN8FK8bbwY0LfhYqvSO8nGcLMdCQbi4aySNIZ7MXNdx79g==";
            
            string encrypted = _transformer.Encrypt(value);
            
            Assert.Equal(expected, encrypted);
        }
        
        [Fact]
        public void TestDecryptLongData()
        {
            const string value = "GfbFiLjsYPaMNrhvWWHpl2xbUSD9Amr0FGTnX7y9DN8FK8bbwY0LfhYqvSO8nGcLMdCQbi4aySNIZ7MXNdx79g==";
            const string expected = "verylongrandomstring1234567890qwertyuiopasdfghjklzxcvbnm";

            string decrypted = _transformer.Decrypt(value);

            Assert.Equal(expected, decrypted);
        }

        [Fact]
        public void TestEncryptNullReturnsNull()
        {
            string encrypt = _transformer.Encrypt(null);

            Assert.Null(encrypt);
        }

        [Fact]
        public void TestDecryptNullReturnsNull()
        {
            string decrypted = _transformer.Decrypt(null);

            Assert.Null(decrypted);
        }
    }
}