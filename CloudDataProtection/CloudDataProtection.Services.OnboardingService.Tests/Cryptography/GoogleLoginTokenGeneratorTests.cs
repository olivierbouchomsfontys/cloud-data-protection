using CloudDataProtection.Core.Cryptography.Generator;
using CloudDataProtection.Services.Onboarding.Cryptography.Generator;
using Xunit;

namespace CloudDataProtection.Services.OnboardingService.Tests.Cryptography
{
    public class GoogleLoginTokenGeneratorTests
    {
        private readonly ITokenGenerator _tokenGenerator;
        
        public GoogleLoginTokenGeneratorTests()
        {
            _tokenGenerator = new GoogleLoginTokenGenerator();
        }
        
        [Fact]
        public void TestGenerate()
        {
            int length = 64;

            string next = _tokenGenerator.Next(length);
            
            Assert.True(next.Length > length);
            
            Assert.DoesNotContain(" ", next);
            Assert.DoesNotContain("=", next);
            Assert.DoesNotContain("+", next);
            Assert.DoesNotContain("/", next);
        }
    }
}