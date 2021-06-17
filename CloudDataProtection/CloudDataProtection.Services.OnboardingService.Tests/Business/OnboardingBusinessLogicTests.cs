using System;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Generator;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Data.Repository;
using CloudDataProtection.Services.Onboarding.Entities;
using CloudDataProtection.Services.Onboarding.Google.Credentials;
using CloudDataProtection.Services.Onboarding.Google.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CloudDataProtection.Services.OnboardingService.Tests.Business
{
    public class OnboardingBusinessLogicTests
    {
        private readonly OnboardingBusinessLogic _logic;

        private readonly Onboarding.Entities.Onboarding _create = new Onboarding.Entities.Onboarding
        {
            UserId = 1,
            Status = OnboardingStatus.None
        };

        private readonly Onboarding.Entities.Onboarding _fetch = new Onboarding.Entities.Onboarding
        {
            UserId = 99,
            Status = OnboardingStatus.None,
            Created = new DateTime(2021, 4, 23)
        };

        private static readonly long _userId = 2;

        public OnboardingBusinessLogicTests()
        {
            var onboardingMock = new Mock<IOnboardingRepository>();
            var loginTokenMock = new Mock<IGoogleLoginTokenRepository>();
            var credentialsMock = new Mock<IGoogleCredentialsRepository>();
            var loggerMock = new Mock<ILogger<OnboardingBusinessLogic>>();

            onboardingMock.Setup(repository => repository.Create(_create))
                .Callback(() => _create.Id = 2)
                .Returns(Task.CompletedTask);

            onboardingMock.Setup(repository => repository.GetByUserId(2))
                .Returns(Task.FromResult(_fetch));
                
            _logic = new OnboardingBusinessLogic
                (onboardingMock.Object, credentialsMock.Object, loginTokenMock.Object, new OtpGenerator(), 
                new GoogleOAuthV2EnvironmentCredentialsProvider(),
                Options.Create(new GoogleOAuthV2Options()), loggerMock.Object);
        }

        #region Onboarding

        [Fact]
        public async Task TestCreateOnboarding_Valid()  
        {
            BusinessResult<Onboarding.Entities.Onboarding> result = await _logic.Create(_create);
            
            Assert.True(_create.Id > 0);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task TestCreateOnboarding_InvalidUserId_ReturnsFalse()
        {
            Onboarding.Entities.Onboarding onboarding = new Onboarding.Entities.Onboarding
            {
                UserId = 0
            };
            
            BusinessResult<Onboarding.Entities.Onboarding> result = await _logic.Create(onboarding);

            Assert.False(result.Success);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task TestGetByUser_Valid()
        {
            BusinessResult<Onboarding.Entities.Onboarding> result = await _logic.GetByUser(_userId);
            
            Assert.True(result.Success);
            
            Assert.Equal(_fetch.Created, result.Data.Created);
            Assert.Equal(_fetch.Id, result.Data.Id);
            Assert.Equal(_fetch.UserId, result.Data.UserId);
            Assert.Equal(_fetch.Status, result.Data.Status);
        }

        [Fact]
        public async Task TestGetByUser_InvalidUserId_ReturnsFalse()
        {
            long userId = -1;
            
            var result = await _logic.GetByUser(userId);
            
            Assert.False(result.Success);
            Assert.NotEmpty(result.Message);
        }

        #endregion

        #region LoginToken

        

        #endregion

        #region GoogleCredentials

        

        #endregion
    }
}