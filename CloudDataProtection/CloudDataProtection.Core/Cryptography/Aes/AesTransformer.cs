using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using CloudDataProtection.Core.Cryptography.Extensions;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Core.Cryptography.Aes
{
    public class AesTransformer : ITransformer
    {
        private readonly AesOptions _options;

        private AesManaged CreateAesManaged() => new AesManaged
        {
            KeySize = _options.KeySize,
            BlockSize = _options.BlockSize,
            Key = _options.Key,
            IV = _options.Iv
        };
        
        public AesTransformer(IOptions<AesOptions> options)
        {
            _options = options.Value;
        }
        
        public string Encrypt(string input)
        {
            if (input == null)
            {
                return null;
            }
            
            byte[] bytes;

            using (System.Security.Cryptography.Aes aes = CreateAesManaged())
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (ICryptoTransform cryptoTransform = aes.CreateEncryptor())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(outputStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            cryptoStream.WriteUtf8String(input);
                            bytes = outputStream.ToArray();
                        }
                    }
                }
            }

            string cipherText = Convert.ToBase64String(bytes, 0, bytes.Length);

            return cipherText;
        }

        public string Decrypt(string input)
        {
            if (input == null)
            {
                return null;
            }

            byte[] bytes;

            using (System.Security.Cryptography.Aes aes = CreateAesManaged())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform cryptoTransform = aes.CreateDecryptor())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            cryptoStream.WriteBase64String(input);
                            bytes = memoryStream.ToArray();
                        }
                    }
                }
            }
            
            string cipherText = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            return cipherText;
        }
    }
}