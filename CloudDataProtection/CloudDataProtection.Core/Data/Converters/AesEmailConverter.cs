using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using CloudDataProtection.Core.Cryptography.Aes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CloudDataProtection.Core.Data.Converters
{
    public class AesEmailConverter : ValueConverter<string, string>
    {
        private static ITransformer _transformer;
        
        public AesEmailConverter(ITransformer transformer) : base(Encrypt, Decrypt)
        {
            _transformer = transformer;
        }

        private static readonly Expression<Func<string, string>> Decrypt = input => 
            ShouldDecrypt(input) 
                ? _transformer.Decrypt(input) 
                : input;
        
        private static readonly Expression<Func<string, string>> Encrypt = input => _transformer.Encrypt(input);

        private static bool ShouldDecrypt(string input) => IsEncryptedData(input);
        private static bool IsEncryptedData(string input) => !new EmailAddressAttribute().IsValid(input);
    }
}