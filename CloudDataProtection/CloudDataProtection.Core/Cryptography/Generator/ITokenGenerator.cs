namespace CloudDataProtection.Core.Cryptography.Generator
{
    public interface ITokenGenerator
    {
        string Next();

        string Next(int bytes);
    }
}