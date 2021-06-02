using System.Linq;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Attributes;
using CloudDataProtection.Core.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CloudDataProtection.Core.Data.Context
{
    public abstract class DbContextBase : DbContext
    {
        private static ITransformer _transformer;

        public DbContextBase()
        {
            
        }

        public DbContextBase(DbContextOptions options) : base(options)
        {
            
        }

        public static void SetTransformer(ITransformer transformer)
        {
            _transformer = transformer;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    object[] attributes = property.PropertyInfo.GetCustomAttributes(typeof(EncryptAttribute), false);
                    
                    if (attributes.Any())
                    {
                        property.SetValueConverter(new AesValueConverter(_transformer));
                    }
                }
            }
        }
    }
}