namespace CloudDataProtection.Dto
{
    public class AuthenticateResult
    {
        public string Token { get; set; }

        public UserResult User { get; set; }
    }
}