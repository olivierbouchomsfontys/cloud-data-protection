using System;
using System.Security.Cryptography;
using CloudDataProtection.Core.Extensions;

namespace CloudDataProtection.Core.Cryptography.Generator
{
    public class OtpGenerator : ITokenGenerator
    {
        private const int DefaultBytes = 64;

        public string Next() => Next(DefaultBytes);

        public string Next(int bytes)
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[bytes];

                rng.GetBytes(tokenData);

                // Workaround to prevent the special base64 characters getting encoded and causing problems in callback
                return Convert.ToBase64String(tokenData)
                    .Remove("+")
                    .Remove("=")
                    .Remove("/");
            }
        }
    }
}