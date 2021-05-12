namespace CloudDataProtection.Services.Onboarding.Messaging.Client.Dto
{
    public class GetUserEmailInput
    {
        public long UserId { get; }

        public GetUserEmailInput(long userId)
        {
            UserId = userId;
        }
    }
}