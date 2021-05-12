using System;
using System.IO;
using System.Security.Cryptography;
using CloudDataProtection.Core.Cryptography.Aes.Options;

namespace CloudDataProtection.Core.Cryptography.Aes
{
    public class AesStreamTransformer : IFileTransformer
    {
        private readonly AesOptions _options;

        private AesManaged CreateAesManaged() => new AesManaged
        {
            KeySize = _options.KeySize,
            BlockSize = _options.BlockSize,
            Key = _options.Key,
            IV = _options.Iv
        };

        public AesStreamTransformer(AesOptions options)
        {
            _options = options;
        }

        public Stream Encrypt(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentException(nameof(input));
            }

            MemoryStream outputStream = new MemoryStream();

            System.Security.Cryptography.Aes aes = CreateAesManaged();
            ICryptoTransform cryptoTransform = aes.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(outputStream, cryptoTransform, CryptoStreamMode.Write);

            input.Position = 0;

            input.CopyTo(cryptoStream);

            cryptoStream.FlushFinalBlock();

            outputStream.Flush();

            outputStream.Position = 0;

            return outputStream;
        }

        public Stream Decrypt(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentException(nameof(input));
            }

            using (System.Security.Cryptography.Aes aes = CreateAesManaged())
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (ICryptoTransform cryptoTransform = aes.CreateDecryptor())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(outputStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            input.Position = 0;

                            input.CopyTo(cryptoStream);

                            cryptoStream.FlushFinalBlock();

                            return outputStream;
                        }
                    }
                }
            }
        }
    }
}