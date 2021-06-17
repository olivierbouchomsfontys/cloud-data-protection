using CloudDataProtection.Core.Cryptography.Generator;
using Xunit;

namespace CloudDataProtection.Services.OnboardingService.Tests.Cryptography
{
    public class OtpGeneratorTests
    {
        private readonly ITokenGenerator _tokenGenerator;
        
        public OtpGeneratorTests()
        {
            _tokenGenerator = new OtpGenerator();
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