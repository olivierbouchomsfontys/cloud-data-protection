namespace CloudDataProtection.Password
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        private const int DefaultWorkFactor = 12;
        
        public bool Match(string hash, string password)
        {
            if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, DefaultWorkFactor);
        }
    }
}   