namespace CloudDataProtection.Email
{
    public class BCryptEmailHasher : IEmailHasher
    {
        private const int DefaultWorkFactor = 12;
        
        public bool Match(string hash, string input)
        {
            if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(input) || string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(input))
            {
                return false;
            }
            
            return BCrypt.Net.BCrypt.Verify(input, hash);

        }

        public string Hash(string email)
        {
            return BCrypt.Net.BCrypt.HashPassword(email, DefaultWorkFactor);
        }
    }
}