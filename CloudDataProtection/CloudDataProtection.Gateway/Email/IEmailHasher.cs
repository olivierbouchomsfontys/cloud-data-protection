namespace CloudDataProtection.Email
{
    public interface IEmailHasher
    {
        bool Match(string hash, string input);

        string Hash(string email);
    }
}