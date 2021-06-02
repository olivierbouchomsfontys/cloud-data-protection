using System;
using System.Linq.Expressions;
using CloudDataProtection.Core.Cryptography.Aes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CloudDataProtection.Core.Data.Converters
{
    public class AesValueConverter : ValueConverter<string, string>
    {
        private static ITransformer _transformer;
        
        public AesValueConverter(ITransformer transformer) : base(Encrypt, Decrypt)
        {
            _transformer = transformer;
        }

        private static Expression<Func<string, string>> Decrypt = input => _transformer.Decrypt(input);
        private static Expression<Func<string, string>> Encrypt = input => _transformer.Encrypt(input);
    }
}