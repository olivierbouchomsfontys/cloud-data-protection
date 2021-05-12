namespace CloudDataProtection.Services.Onboarding.Messaging.Publisher.Dto
{
    public class GoogleAccountConnectedModel
    {
        public long UserId { get; }
        
        public string Email { get; }

        public GoogleAccountConnectedModel(long userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}