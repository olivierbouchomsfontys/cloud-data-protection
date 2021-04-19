namespace CloudDataProtection.Services.Onboarding.Dto
{
    public class UserRegisteredModel
    {
        public long Id { get; set; }
        
        public string Email { get; set; }
        
        public UserRegisteredRole Role { get; set; }
    }

    public enum UserRegisteredRole
    {
        Client = 0,
        Employee = 1
    }
}