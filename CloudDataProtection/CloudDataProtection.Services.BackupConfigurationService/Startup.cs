using AutoMapper;
using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Jwt.Options;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Subscription.Business;
using CloudDataProtection.Services.Subscription.Data.Context;
using CloudDataProtection.Services.Subscription.Data.Repository;
using CloudDataProtection.Services.Subscription.Dto;
using CloudDataProtection.Services.Subscription.Entities;
using CloudDataProtection.Services.Subscription.Messaging.Dto;
using CloudDataProtection.Services.Subscription.Messaging.Publisher;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CloudDataProtection.Services.Subscription
{
    public class Startup
    {
        private static readonly string CorsPolicy = "cors-policy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IBackupConfigurationRepository, BackupConfigurationRepository>();
            services.AddScoped<IBackupSchemeRepository, BackupSchemeRepository>();

            services.AddLazy<BackupSchemeBusinessLogic>();
            services.AddLazy<BackupConfigurationBusinessLogic>();
            
            services.Configure<RabbitMqConfiguration>(options => Configuration.GetSection("RabbitMq").Bind(options));
            
            services.AddLazy<IMessagePublisher<BackupConfigurationEnteredModel>, BackupConfigurationEnteredMessagePublisher>();
            
            services.AddEncryptedDbContext<IBackupConfigurationDbContext, BackupConfigurationEncryptedDbContext>(Configuration, builder =>
            {
                builder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                {
                    builder
                        .WithOrigins("https://localhost:5021", "https://localhost:5001")
                        .AllowAnyOrigin()
                        .AllowAnyMethod();
                });
            });
            
            ConfigureAuthentication(services);

            services.AddAutoMapper(ConfigureMapper);
        }

        private void ConfigureMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<BackupScheme, BackupSchemeResult>()
                .ForMember(p => p.Hour, 
                    options => options.MapFrom(s => s.Time.Hours))
                .ForMember(p => p.Minute, 
                    options => options.MapFrom(s => s.Time.Minutes));
            
            config.CreateMap<BackupConfiguration, BackupConfigurationResult>()
                .ForMember(p => p.Hour, 
                    options => options.MapFrom(s => s.Time.Hours))
                .ForMember(p => p.Minute, 
                    options => options.MapFrom(s => s.Time.Minutes));
            
            config.CreateMap<BackupConfiguration, CreateBackupConfigurationResult>()
                .ForMember(p => p.Hour, 
                    options => options.MapFrom(s => s.Time.Hours))
                .ForMember(p => p.Minute, 
                    options => options.MapFrom(s => s.Time.Minutes));

            config.CreateMap<BackupScheme, BackupConfiguration>()
                .ForMember(p => p.Id,
                    options => options.Ignore())
                .ForMember(p => p.CreatedAt,
                    options => options.Ignore())
                .ForMember(p => p.UserId,
                    options => options.Ignore())
                .ForMember(p => p.TimeId,
                    options => options.Ignore())
                .ForMember(p => p.Time,
                    options => options.Ignore())
                .ForPath(p => p.Time.Hours,
                    options => options.MapFrom(s => s.Time.Hours))
                .ForPath(p => p.Time.Minutes,
                    options => options.MapFrom(s => s.Time.Minutes))
                .ForPath(p => p.Time.Seconds,
                    options => options.MapFrom(s => s.Time.Seconds));
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            JwtSecretOptions options = new JwtSecretOptions();
            
            Configuration.GetSection("Jwt").Bind(options);
            
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(options.Key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddScoped<IJwtDecoder, JwtDecoder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}