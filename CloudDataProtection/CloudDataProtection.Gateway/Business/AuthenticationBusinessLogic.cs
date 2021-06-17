using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Business.Options;
using CloudDataProtection.Core.Cryptography.Generator;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Data;
using CloudDataProtection.Entities;
using CloudDataProtection.Password;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Business
{
    public class AuthenticationBusinessLogic
    {
        private readonly IAuthenticationRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ChangeEmailOptions _changeEmailOptions;

        private const int MinimumPasswordLength = 8;

        public AuthenticationBusinessLogic(IAuthenticationRepository repository, IPasswordHasher passwordHasher, IOptions<ChangeEmailOptions> changeEmailOptions, ITokenGenerator tokenGenerator)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _changeEmailOptions = changeEmailOptions.Value;
        }

        public async Task<BusinessResult<User>> Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BusinessResult<User>.Error("Invalid username or password");
            }
            
            User user = await _repository.Get(username);

            if (user == null)
            {
                return BusinessResult<User>.Error("Invalid username or password");
            }

            if (!_passwordHasher.Match(user.Password, password))
            {
                return BusinessResult<User>.Error("Invalid username or password");
            }

            return BusinessResult<User>.Ok(user);
        }

        public async Task<BusinessResult<User>> Get(int id)
        {
            User user = await _repository.Get(id);

            if (user == null)
            {
                return BusinessResult<User>.Error($"Could not find user with id = {id}");
            }
            
            return BusinessResult<User>.Ok(user);
        }

        public async Task<BusinessResult<User>> Get(string email)
        {
            User user = await _repository.Get(email);

            if (user == null)
            {
                return BusinessResult<User>.Error($"Could not find user with email = {email}");
            }
            
            return BusinessResult<User>.Ok(user);
        }

        public async Task<BusinessResult<User>> Create(User user, string password)
        {
            if (user.Email == null || !new EmailAddressAttribute().IsValid(user.Email))
            {
                return BusinessResult<User>.Error("Invalid email provided");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < MinimumPasswordLength)
            {
                return BusinessResult<User>.Error($"Password must be at least {MinimumPasswordLength} characters long");
            }
            
            if (await _repository.Get(user.Email) != null)
            {
                return BusinessResult<User>.Error($"A user with email {user.Email} already exists");
            }

            user.Password = _passwordHasher.HashPassword(password);
            
            await _repository.Create(user);

            return BusinessResult<User>.Ok(user);
        }
        
        public async Task<BusinessResult<ChangeEmailRequest>> RequestChangeEmail(long userId, string newEmail)
        {
            if (newEmail == null || !new EmailAddressAttribute().IsValid(newEmail))
            {
                return BusinessResult<ChangeEmailRequest>.Error("Invalid email provided");
            }
            
            User user = await _repository.Get(userId);

            if (user == null)
            {
                return BusinessResult<ChangeEmailRequest>.Error($"Could not find user with id = {userId}");
            }

            if (user.Email.Equals(newEmail, StringComparison.OrdinalIgnoreCase))
            {
                return BusinessResult<ChangeEmailRequest>.Error("Email address is same as current");
            }

            bool emailExists = await _repository.Get(newEmail) != null;

            if (emailExists)
            {
                return BusinessResult<ChangeEmailRequest>.Error("Email address is already in use");
            }

            IEnumerable<ChangeEmailRequest> requestsByEmail = await _repository.GetAll(newEmail);

            // There is already a request to change to this email address
            if (requestsByEmail.Any(c => c.IsValid))
            {
                return BusinessResult<ChangeEmailRequest>.Error("Email address is already in use");
            }

            IEnumerable<ChangeEmailRequest> oldRequests = await _repository.GetAll(userId);
            
            ICollection<ChangeEmailRequest> validRequests = oldRequests.Where(r => r.IsValid).ToList();

            if (validRequests.Any())
            {
                foreach (ChangeEmailRequest changeEmailRequest in validRequests)
                {
                    changeEmailRequest.Invalidate();
                }

                await _repository.Update(validRequests);
            }

            ChangeEmailRequest request = new ChangeEmailRequest
            {
                UserId = userId,
                NewEmail = newEmail,
                ExpiresAt = DateTime.Now.AddMinutes(_changeEmailOptions.ExpiresInMinutes),
                Token = _tokenGenerator.Next()
            };

            await _repository.Create(request);

            return BusinessResult<ChangeEmailRequest>.Ok(request);
        }

        public async Task<BusinessResult<string>> ConfirmChangeEmail(string inputToken)
        {
            if (string.IsNullOrEmpty(inputToken))
            {
                return BusinessResult<string>.Error("No token has been provided");
            }

            ChangeEmailRequest request = await _repository.GetEmailRequest(inputToken);

            if (request == null)
            {
                return BusinessResult<string>.Error("An invalid token has been provided");
            }

            if (!request.IsValid)
            {
                return BusinessResult<string>.Error("The token has expired or is already used");
            }

            User user = await _repository.Get(request.UserId);

            if (user == null)
            {
                return BusinessResult<string>.Error("An invalid token has been provided");
            }

            user.Email = request.NewEmail;

            await _repository.Update(user);
            
            return BusinessResult<string>.Ok(user.Email);
        }
    }
}