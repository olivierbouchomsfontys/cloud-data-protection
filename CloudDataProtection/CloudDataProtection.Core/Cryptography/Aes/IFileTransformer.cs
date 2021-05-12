using System.IO;

namespace CloudDataProtection.Core.Cryptography.Aes
{
    public interface IFileTransformer
    {
        Stream Encrypt(Stream input);
        Stream Decrypt(Stream input);
    }
}