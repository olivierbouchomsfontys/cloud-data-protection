using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Controllers.Data;
using CloudDataProtection.Core.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;

namespace CloudDataProtection.Core.Tests.Mocks
{
    public class MockServiceController : ServiceController
    {
        public MockServiceController(IJwtDecoder jwtDecoder) : base(jwtDecoder)
        {
        }

        public new long UserId => base.UserId;

        public new UserRole? UserRole => base.UserRole;

        public new HttpRequest Request => new DefaultHttpRequest(new DefaultHttpContext(new FeatureCollection()));
    }
}