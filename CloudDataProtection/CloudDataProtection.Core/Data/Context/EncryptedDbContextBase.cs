using System.Linq;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Attributes;
using CloudDataProtection.Core.Data.Converters;
using CloudDataProtection.Core.Papertrail.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Core.Data.Context
{
    public abstract class EncryptedDbContextBase : DbContext
    {
        private static ILoggerFactory _loggerFactory;
        private static ILoggerFactory LoggerFactory => _loggerFactory ??= CreateLoggerFactory();

        private static ILoggerFactory CreateLoggerFactory()
        {
            return Microsoft.Extensions.Logging.LoggerFactory.Create(builder => builder.ConfigureLogging());
        }

        private readonly ITransformer _transformer;

        public EncryptedDbContextBase()
        {
        }

        public EncryptedDbContextBase(DbContextOptions options, ITransformer transformer) : base(options)
        {
            _transformer = transformer;
        }

        protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // The transformer won't be set if the ef core tools are running
            if (_transformer == null)
            {
                return;
            }

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    object attributes = property.PropertyInfo.GetCustomAttributes(typeof(EncryptAttribute), false)
                        .FirstOrDefault();

                    if (attributes is EncryptAttribute encryptAttribute)
                    {
                        switch (encryptAttribute.DataType)
                        {
                            case DataType.EmailAddress:
                                property.SetValueConverter(new AesEmailConverter(_transformer));
                                break;
                            case DataType.Unknown:
                                property.SetValueConverter(new AesValueConverter(_transformer));
                                break;
                        }
                    }
                }
            }
        }

        protected sealed override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder.UseLoggerFactory(LoggerFactory);

            if (builder.IsConfigured)
            {
                return;
            }

            ConfigureForEfCoreTools(builder);
        }

        /// <summary>
        /// Configures DbContext for Entity Framework tools
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void ConfigureForEfCoreTools(DbContextOptionsBuilder builder);
    }
}