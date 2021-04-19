using System.Reflection;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Data;
using CloudDataProtection.Entities;
using CloudDataProtection.Password;
using Moq;
using Xunit;

namespace CloudDataProtection.Gateway.Tests.Business
{
    public class AuthenticationBusinessLogicTests
    {
        private readonly AuthenticationBusinessLogic _logic;

        private readonly User _register = new User
        {
            Email = "info@example.com",
            Password = "password"
        };
        
        private readonly User _fetch = new User
        {
            Id = 1,
            Email = "test@example.com",
            // Password: password
            Password = "$2a$12$13vDgPeLSGdVMpCgs/jpquS1r/TOogVPS6nFDL1P8j4JWmFw3Q3XO"
        };

        public AuthenticationBusinessLogicTests()
        {
            var mock = new Mock<IAuthenticationRepository>();
            
            mock.Setup(repository => repository.Get(1))
                .Returns(Task.FromResult(_fetch));
            
            mock.Setup(repository => repository.Get("test@example.com"))
                .Returns(Task.FromResult(_fetch));

            mock.Setup(repository => repository.Create(_register))
                .Callback(() => _register.Id = 2)
                .Returns(Task.CompletedTask);
            
            _logic = new AuthenticationBusinessLogic(mock.Object, new BCryptPasswordHasher());
        }

        #region Valid credentials

        [Fact]
        public async Task TestAuthenticate_ValidCredentials()
        {
            string email = "test@example.com";
            string password = "password";
            
            BusinessResult<User> authenticateResult = await _logic.Authenticate(email, password);

            Assert.True(authenticateResult.Success);
            Assert.Equal(email, authenticateResult.Data.Email);
            Assert.NotEqual(password, authenticateResult.Data.Password);

            Assert.Equal(_fetch.Email, authenticateResult.Data.Email);
            Assert.Equal(_fetch.Id, authenticateResult.Data.Id);
            Assert.Equal(_fetch.Password, authenticateResult.Data.Password);
        }

        [Fact]
        public async Task TestCreate_ValidCredentials()
        {
            var registerResult = await _logic.Create(_register, "password");
            
            Assert.True(registerResult.Success);
            Assert.Equal(_register.Email, registerResult.Data.Email);
            Assert.Equal(_register.Password, registerResult.Data.Password);
            Assert.True(registerResult.Data.Id > 0);
        }

        [Fact]
        public async Task TestCreate_ExistingEmail()
        {
            User user = new User
            {
                Email = _fetch.Email
            };
            
            var registerResult = await _logic.Create(user, "password");
            
            Assert.False(registerResult.Success);
            Assert.Null(registerResult.Data);
            Assert.NotEmpty(registerResult.Message);
        }

        [Fact]
        public async Task TestGet_ExistingId()
        {
            BusinessResult<User> user = await _logic.Get(1);
            
            Assert.True(user.Success);
            Assert.NotNull(user.Data);
            Assert.True(user.Data.Id > 0);
            Assert.NotEmpty(user.Data.Email);
            Assert.NotEmpty(user.Data.Password);
        }

        #endregion

        #region Invalid credentials
        
        [Fact]
        public async Task TestAuthenticate_InvalidEmail()
        {
            string email = "invalidemail@example.com";
            string password = "password";
            
            BusinessResult<User> authenticateResult = await _logic.Authenticate(email, password);

            Assert.False(authenticateResult.Success);
            Assert.NotEmpty(authenticateResult.Message);
            Assert.Null(authenticateResult.Data);
        }

        [Fact]
        public async Task TestAuthenticate_InvalidPassword()
        {
            string email = "test@example.com";
            string password = "invalidpassword";
            
            BusinessResult<User> authenticateResult = await _logic.Authenticate(email, password);

            Assert.False(authenticateResult.Success);
            Assert.NotEmpty(authenticateResult.Message);
            Assert.Null(authenticateResult.Data);
        }
        
        [Fact]
        public async Task TestCreate_InvalidEmail()
        {
            User user = new User
            {
                Email = "invalid"
            };
            
            var registerResult = await _logic.Create(user, "password");
            
            Assert.False(registerResult.Success);
            Assert.Null(registerResult.Data);
            Assert.NotEmpty(registerResult.Message);
        }

        #endregion

        #region Incomplete credentials
        
        [Fact]
        public async Task TestAuthenticate_EmptyUsername()
        {
            string email = string.Empty;
            string password = "password";
            
            BusinessResult<User> authenticateResult = await _logic.Authenticate(email, password);

            Assert.False(authenticateResult.Success);
            Assert.NotEmpty(authenticateResult.Message);
            Assert.Null(authenticateResult.Data);
        }
        
        [Fact]
        public async Task TestAuthenticate_NullUsername()
        {
            string password = "password";
            
            BusinessResult<User> authenticateResult = await _logic.Authenticate(null, password);

            Assert.False(authenticateResult.Success);
            Assert.NotEmpty(authenticateResult.Message);
            Assert.Null(authenticateResult.Data);
        }
        
        [Fact]
        public async Task TestAuthenticate_EmptyPassword()
        {
            string email = "test@example.com";
            string password = string.Empty;
            
            BusinessResult<User> authenticateResult = await _logic.Authenticate(email, password);

            Assert.False(authenticateResult.Success);
            Assert.NotEmpty(authenticateResult.Message);
            Assert.Null(authenticateResult.Data);
        }
        
        [Fact]
        public async Task TestAuthenticate_NullPassword()
        {
            string email = "test@example.com";
            
            BusinessResult<User> authenticateResult = await _logic.Authenticate(email, null);

            Assert.False(authenticateResult.Success);
            Assert.NotEmpty(authenticateResult.Message);
            Assert.Null(authenticateResult.Data);
        }

        [Fact]
        public async Task TestAuthenticate_EmptyCredentials()
        {
            string email = string.Empty;
            string password = string.Empty;
            
            BusinessResult<User> authenticateResult = await _logic.Authenticate(email, password);

            Assert.False(authenticateResult.Success);
            Assert.NotEmpty(authenticateResult.Message);
            Assert.Null(authenticateResult.Data);
        }

        [Fact]
        public async Task TestAuthenticate_NullCredentials()
        {
            BusinessResult<User> authenticateResult = await _logic.Authenticate(null, null);

            Assert.False(authenticateResult.Success);
            Assert.NotEmpty(authenticateResult.Message);
            Assert.Null(authenticateResult.Data);
        }

        [Fact]
        public async Task TestCreate_EmptyEmail()
        {
            User user = new User
            {
                Email = string.Empty
            };
            
            var registerResult = await _logic.Create(user, "password");
            
            Assert.False(registerResult.Success);
            Assert.Null(registerResult.Data);
            Assert.NotEmpty(registerResult.Message);
        }

        [Fact]
        public async Task TestCreate_NullEmail()
        {
            User user = new User
            {
                Email = string.Empty
            };
            
            var registerResult = await _logic.Create(user, "password");
            
            Assert.False(registerResult.Success);
            Assert.Null(registerResult.Data);
            Assert.NotEmpty(registerResult.Message);
        }


        [Fact]
        public async Task TestCreate_EmptyPassword()
        {
            var registerResult = await _logic.Create(_fetch, string.Empty);
            
            Assert.False(registerResult.Success);
            Assert.Null(registerResult.Data);
            Assert.NotEmpty(registerResult.Message);
        }

        [Fact]
        public async Task TestCreate_NullPassword()
        {
            var registerResult = await _logic.Create(_fetch, null);
            
            Assert.False(registerResult.Success);
            Assert.Null(registerResult.Data);
            Assert.NotEmpty(registerResult.Message);
        }
        
        [Fact]
        public async Task TestGet_UnknownId()
        {
            BusinessResult<User> result = await _logic.Get(-1);
            
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        #endregion
    }
}