namespace CloudDataProtection.Password
{
    public interface IPasswordHasher
    {
        bool Match(string hash, string input);

        string HashPassword(string password);
    }
}