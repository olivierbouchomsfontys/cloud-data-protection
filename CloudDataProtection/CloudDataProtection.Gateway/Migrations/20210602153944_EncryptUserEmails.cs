using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Data.Context;
using CloudDataProtection.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Migrations
{
    public partial class EncryptUserEmails : Migration
    {
        /// <summary>
        /// Encrypts all the user email addresses by forcefully updating the data
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string environment = EnvironmentVariableHelper.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();

            AesOptions options = new AesOptions();
            
            configuration.GetSection("Persistence").Bind(options);
            
            ITransformer transformer = new AesTransformer(Options.Create(options));

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            
            using (AuthenticationDbContext context = new AuthenticationDbContext(builder.Options, transformer))
            {
                List<User> allUsers = context.User.ToList();
                
                allUsers.ForEach(u =>
                {
                    context.Entry(u).State = EntityState.Modified;
                });

                context.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
