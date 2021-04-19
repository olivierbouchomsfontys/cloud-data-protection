using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Dto;
using CloudDataProtection.Entities;

namespace CloudDataProtection.Seeder
{
    public class UserSeeder
    {
        private readonly AuthenticationBusinessLogic _logic;
        private readonly IMessagePublisher<UserResult> _messagePublisher;
        
        private static readonly PasswordGenerator.Password Password = new PasswordGenerator.Password();

        private static readonly User User = new User
        {
            Email = "cloudsnapshotter@gmail.com",
            Role = UserRole.Employee,
            Password = Password.Next()
        };

        public UserSeeder(AuthenticationBusinessLogic logic, IMessagePublisher<UserResult> publisher)
        {
            _logic = logic;
            _messagePublisher = publisher;
        }

        public async Task Seed()
        {
            var getResult = await _logic.Get(User.Email);

            if (getResult.Success && getResult.Data != null)
            {
                return;
            }
            
            BusinessResult<User> businessResult = await _logic.Create(User, Password.Next());

            if (businessResult.Success)
            {
                UserResult result = new UserResult
                {
                    Email = User.Email,
                    Id = User.Id
                };

                await _messagePublisher.Send(result);
            }
        }
    }
}