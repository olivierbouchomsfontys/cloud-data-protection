using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Data;
using CloudDataProtection.Entities;
using CloudDataProtection.Password;

namespace CloudDataProtection.Business
{
    public class UserBusinessLogic
    {
        private readonly IAuthenticationRepository _repository;
        private readonly IUserHistoryRepository _userHistoryRepository;

        private readonly string[] ServicesToDeleteData = new[] {"Onboarding", "Gateway", "BackupConfiguration"};

        public UserBusinessLogic(IAuthenticationRepository repository, IUserHistoryRepository userHistoryRepository, IPasswordHasher hasher)
        {
            _repository = repository;
            _userHistoryRepository = userHistoryRepository;
        }
        
        public async Task<BusinessResult<User>> Get(long id)
        {
            User user = await _repository.Get(id);

            if (user == null)
            {
                return BusinessResult<User>.Error($"Could not find user with id = {id}");
            }
            
            return BusinessResult<User>.Ok(user);
        }

        public async Task<BusinessResult<User>> Delete(long id)
        {
            DateTime start = DateTime.Now;
            
            User user = await _repository.Get(id);

            if (user == null)
            {
                return BusinessResult<User>.Error($"Could not find user with id = {id}");
            }

            await _repository.Delete(user);

            UserDeletionHistoryProgress current = new UserDeletionHistoryProgress
            {
                ServiceName = "Gateway",
                StartedAt = start,
                CompletedAt = DateTime.Now
            };
            
            UserDeletionHistory history = new UserDeletionHistory
            {
                Email = user.Email,
                UserId = user.Id,
                Progress = new List<UserDeletionHistoryProgress>() {current}
            };

            await _userHistoryRepository.RegisterDelete(history);
            
            return BusinessResult<User>.Ok(user);
        }

        public async Task<BusinessResult<UserDeletionHistory>> AddProgress(UserDeletionHistoryProgress progress, long userId)
        {
            UserDeletionHistory history = await _userHistoryRepository.GetDelete(userId);
            
            history.Progress.Add(progress);
                
            IEnumerable<string> removed = history.Progress.Select(p => p.ServiceName);

            bool isComplete = removed.All(r => ServicesToDeleteData.Contains(r));

            if (isComplete)
            {
                history.CompletedAt = DateTime.Now;
            }

            await _userHistoryRepository.Update(history);

            return BusinessResult<UserDeletionHistory>.Ok(history);
        }
    }
}