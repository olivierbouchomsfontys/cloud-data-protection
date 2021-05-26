using CloudDataProtection.Core.Controllers.Data;
using CloudDataProtection.Core.Jwt;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace CloudDataProtection.Core.Tests.Jwt
{
    public class JwtDecoderTest
    {
        private readonly IJwtDecoder _jwtDecoder;
        
        private const int UserId = 11;
        private const UserRole UserRole = Controllers.Data.UserRole.Client;
        
        private const string Jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjExIiwiZW1haWwiOiJjbG91ZHNuYXBzaG90dGVyK3Rlc3RAZ21haWwuY29tIiwicm9sZSI6IjAiLCJuYmYiOjE2MjE2MTM4MzksImV4cCI6MTYyMjgyMzQzOSwiaWF0IjoxNjIxNjEzODM5fQ.6dk-QaiUkJ6CAm4SjjNnaETkzQJrQsiW2NSw42kTw84";

        public JwtDecoderTest()
        {
            _jwtDecoder = new JwtDecoder();
        }

        [Fact]
        public void TestGetUserData()
        {
            IHeaderDictionary dictionary = new HeaderDictionary();
            
            dictionary.Add("Authorization", $"Bearer {Jwt}");

            long? userId = _jwtDecoder.GetUserId(dictionary);
            UserRole? userRole = _jwtDecoder.GetUserRole(dictionary);
            
            Assert.Equal(UserId, userId);
            Assert.Equal(UserRole, userRole);
        }

        [Fact]
        public void TestGetUserDataEmpty()
        {
            IHeaderDictionary dictionary = new HeaderDictionary();

            long? userId = _jwtDecoder.GetUserId(dictionary);
            UserRole? userRole = _jwtDecoder.GetUserRole(dictionary);

            Assert.Null(userId);
            Assert.Null(userRole);
        }

        [Fact]
        public void TestGetUserDataNull()
        {
            IHeaderDictionary dictionary = null;

            long? userId = _jwtDecoder.GetUserId(dictionary);
            UserRole? userRole = _jwtDecoder.GetUserRole(dictionary);

            Assert.Null(userId);
            Assert.Null(userRole);
        }
    }
}