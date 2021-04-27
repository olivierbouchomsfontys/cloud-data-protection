using CloudDataProtection.Core.Controllers.Data;
using CloudDataProtection.Core.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Core.Controllers
{
    public abstract class ServiceController : ControllerBase
    {
        private readonly IJwtDecoder _jwtDecoder;

        private bool _initialized;

        protected ServiceController(IJwtDecoder jwtDecoder)
        {
            _jwtDecoder = jwtDecoder;
        }

        private void Initialize()
        {
            _userId = _jwtDecoder.GetUserId(Request.Headers);
            _userRole = _jwtDecoder.GetUserRole(Request.Headers);

            _initialized = true;
        }

        private long? _userId;

        /// <summary>
        /// Id of the current user
        /// </summary>
        protected long UserId
        {
            get
            {
                if (!_initialized)
                {
                    Initialize();
                }

                return _userId.GetValueOrDefault();
            }
        }

        private UserRole? _userRole;

        public UserRole? UserRole
        {
            get
            {
                if (!_initialized)
                {
                    Initialize();
                }

                return _userRole;
            }
        }

        /// <summary>
        /// If a user is authenticated
        /// </summary>
        protected bool IsAuthenticated => UserId > 0;
    }
}