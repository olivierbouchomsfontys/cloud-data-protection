using System.IO;

namespace CloudDataProtection.Core.Cryptography.Aes
{
    public interface IDataTransformer
    {
        Stream Encrypt(Stream input);
        byte[] Decrypt(Stream input);
    }
}