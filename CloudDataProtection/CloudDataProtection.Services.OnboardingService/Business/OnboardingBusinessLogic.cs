using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Generator;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Data.Repository;
using CloudDataProtection.Services.Onboarding.Entities;
using CloudDataProtection.Services.Onboarding.Google.Credentials;
using CloudDataProtection.Services.Onboarding.Google.Dto;
using CloudDataProtection.Services.Onboarding.Google.Options;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CloudDataProtection.Services.Onboarding.Business
{
    public class OnboardingBusinessLogic
    {
        private readonly IOnboardingRepository _onboardingRepository;
        private readonly IGoogleCredentialsRepository _credentialsRepository;
        private readonly IGoogleLoginTokenRepository _loginTokenRepository;
        private readonly ITokenGenerator _tokenGenerator;

        private readonly IGoogleOAuthV2CredentialsProvider _credentialsProvider;
        private readonly ILogger<OnboardingBusinessLogic> _logger;
        private readonly GoogleOAuthV2Options _oAuthV2Options;

        public OnboardingBusinessLogic(IOnboardingRepository onboardingRepository, 
            IGoogleCredentialsRepository credentialsRepository, 
            IGoogleLoginTokenRepository loginTokenRepository, 
            ITokenGenerator tokenGenerator,
            IGoogleOAuthV2CredentialsProvider credentialsProvider,
            IOptions<GoogleOAuthV2Options> oauthV2Options,
            ILogger<OnboardingBusinessLogic> logger)
        {
            _onboardingRepository = onboardingRepository;
            _credentialsRepository = credentialsRepository;
            _loginTokenRepository = loginTokenRepository;
            _tokenGenerator = tokenGenerator;
            _credentialsProvider = credentialsProvider;
            _oAuthV2Options = oauthV2Options.Value;
            _logger = logger;
        }

        public async Task<BusinessResult<Entities.Onboarding>> GetByUser(long userId)
        {
            if (userId <= 0)
            {
                return BusinessResult<Entities.Onboarding>.Error("User Id was not set for onboarding");
            }
            
            Entities.Onboarding onboarding = await _onboardingRepository.GetByUserId(userId);

            if (onboarding == null)
            {
                Entities.Onboarding newOnboarding = new Entities.Onboarding
                {
                    UserId = userId,
                    Status = OnboardingStatus.None
                };

                await Create(newOnboarding);

                return await GetByUser(userId);
            }

            return BusinessResult<Entities.Onboarding>.Ok(onboarding);
        }
        
        public async Task<BusinessResult<Entities.Onboarding>> Create(Entities.Onboarding onboarding)
        {
            if (onboarding.UserId <= 0)
            {
                return BusinessResult<Entities.Onboarding>.Error("User Id was not set for onboarding");
            }

            await _onboardingRepository.Create(onboarding);

            return BusinessResult<Entities.Onboarding>.Ok(onboarding);
        }
        
        public async Task<BusinessResult<Entities.Onboarding>> Update(Entities.Onboarding onboarding)
        {
            await _onboardingRepository.Update(onboarding);
            
            return BusinessResult<Entities.Onboarding>.Ok(onboarding);
        }

        public async Task<BusinessResult<GoogleCredentials>> CreateCredentials(string code, string token)
        {
            GoogleLoginToken loginToken = await _loginTokenRepository.Get(token);

            if (loginToken == null || !loginToken.IsValid)
            {
                return BusinessResult<GoogleCredentials>.Error("Google login token is invalid.");
            }
            
            loginToken.Invalidate();
            
            GoogleOAuthV2Request request = new GoogleOAuthV2Request
            {
                code = code,
                client_id = _credentialsProvider.ClientId,
                client_secret = _credentialsProvider.ClientSecret,
                redirect_uri = _oAuthV2Options.RedirectUri,
                grant_type = _oAuthV2Options.GrantType
            };
            
            _logger.LogInformation("Sending OAuthV2 request: {Request}", JsonConvert.SerializeObject(request.HidePII()));
            
            IFlurlResponse response = await _oAuthV2Options.Endpoint.PostUrlEncodedAsync(request);

            GoogleOAuthV2Response responseBody = await response.GetJsonAsync<GoogleOAuthV2Response>();

            if (responseBody.RefreshToken == null)
            {
                return BusinessResult<GoogleCredentials>.Error("No refresh token has been returned. The Google account is probably already connected to Cloud Data Protection.");
            }

            GoogleCredentials credentials = new GoogleCredentials
            {
                RefreshToken = responseBody.RefreshToken,
                UserId = loginToken.UserId
            };

            Entities.Onboarding onboarding = await _onboardingRepository.GetByUserId(loginToken.UserId);

            onboarding.Status = OnboardingStatus.AccountConnected;

            await _loginTokenRepository.Update(loginToken);
            await _credentialsRepository.Create(credentials);
            await _onboardingRepository.Update(onboarding);

            return BusinessResult<GoogleCredentials>.Ok(credentials);
        }
        
        public async Task<BusinessResult<GoogleLoginInfo>> GetLoginInfo(long userId)
        {
            GoogleLoginToken newToken = new GoogleLoginToken
            {
                UserId = userId,
                Token = _tokenGenerator.Next()
            };

            var tokens = await _loginTokenRepository.GetAllByUser(userId);

            var tokensToInvalidate = tokens.Where(t => t.IsValid).ToList();
            
            foreach (GoogleLoginToken token in tokensToInvalidate)
            {
                token.Invalidate();
            }

            await _loginTokenRepository.Update(tokensToInvalidate);

            await _loginTokenRepository.Create(newToken);

            GoogleLoginInfo info = new GoogleLoginInfo
            {
                State = newToken.Token,
                ClientId = _credentialsProvider.ClientId,
                RedirectUri = _oAuthV2Options.RedirectUri,
                Scopes = _oAuthV2Options.Scopes
            };

            return BusinessResult<GoogleLoginInfo>.Ok(info);
        }
    }
}