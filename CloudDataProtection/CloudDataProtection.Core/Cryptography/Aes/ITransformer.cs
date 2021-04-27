namespace CloudDataProtection.Core.Cryptography.Aes
{
    public interface ITransformer
    {
        string Encrypt(string input);
        string Decrypt(string input);
    }
}