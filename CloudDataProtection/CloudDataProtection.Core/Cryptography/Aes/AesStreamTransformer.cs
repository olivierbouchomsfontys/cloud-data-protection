using System;
using System.IO;
using System.Security.Cryptography;
using CloudDataProtection.Core.Cryptography.Aes.Options;

namespace CloudDataProtection.Core.Cryptography.Aes
{
    public class AesStreamTransformer : IDataTransformer
    {
        private readonly AesOptions _options;

        private AesManaged CreateAesManaged() => new AesManaged
        {
            KeySize = _options.KeySize,
            BlockSize = _options.BlockSize,
            Key = _options.Key,
            IV = _options.Iv
        };

        private const int DefaultBufferSize = 2048;
        
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

        public byte[] Decrypt(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentException(nameof(input));
            }

            using (MemoryStream outputStream = new MemoryStream())
            {
                using (System.Security.Cryptography.Aes aes = CreateAesManaged())
                {
                    using (ICryptoTransform cryptoTransform = aes.CreateDecryptor())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(outputStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            byte[] buffer;

                            if (input.CanSeek)
                            {
                                buffer = new byte[Math.Min(DefaultBufferSize, input.Length)];
                                input.Seek(0, SeekOrigin.Begin);
                            }
                            else
                            {
                                buffer = new byte[DefaultBufferSize];
                            }

                            int read;
                            
                            while((read = input.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cryptoStream.Write(buffer, 0, read);
                            }
                            
                            cryptoStream.FlushFinalBlock();
            
                            return outputStream.ToArray();
                        }
                    }
                }
            }
        }
    }
}