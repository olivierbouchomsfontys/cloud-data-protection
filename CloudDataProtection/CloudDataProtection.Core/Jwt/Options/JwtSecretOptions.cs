using System.Text;

namespace CloudDataProtection.Core.Jwt.Options
{
    public class JwtSecretOptions
    {
        public string Secret { get; set; }

        public byte[] Key => Encoding.ASCII.GetBytes(Secret);
    }
}