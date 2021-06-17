namespace CloudDataProtection.Dto.Result
{
    public class AuthenticateResult
    {
        public string Token { get; set; }

        public UserResult User { get; set; }
    }
}