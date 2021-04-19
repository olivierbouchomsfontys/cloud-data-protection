using CloudDataProtection.Core.Controllers.Data;
using CloudDataProtection.Core.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Core.Controllers
{
    public abstract class ServiceController : ControllerBase
    {
        private readonly IJwtDecoder _jwtDecoder;

        protected ServiceController(IJwtDecoder jwtDecoder)
        {
            _jwtDecoder = jwtDecoder;

            Initialize();
        }

        private void Initialize()
        {
            _userId = _jwtDecoder.GetUserId(Request.Headers);
            _userRole = _jwtDecoder.GetUserRole(Request.Headers);
        }

        private long? _userId;

        /// <summary>
        /// Id of the current user
        /// </summary>
        protected long UserId => _userId.GetValueOrDefault();

        private UserRole? _userRole;

        public UserRole UserRole => _userRole.GetValueOrDefault();

        /// <summary>
        /// If a user is authenticated
        /// </summary>
        protected bool IsAuthenticated => _userId.HasValue;
    }
}