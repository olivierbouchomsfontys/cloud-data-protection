using AutoMapper;
using CloudDataProtection.Core.Cryptography.Generator;
using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Jwt.Options;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Config;
using CloudDataProtection.Services.Onboarding.Data.Context;
using CloudDataProtection.Services.Onboarding.Data.Repository;
using CloudDataProtection.Services.Onboarding.Dto;
using CloudDataProtection.Services.Onboarding.Entities;
using CloudDataProtection.Services.Onboarding.Google.Credentials;
using CloudDataProtection.Services.Onboarding.Google.Options;
using CloudDataProtection.Services.Onboarding.Messaging.Client;
using CloudDataProtection.Services.Onboarding.Messaging.Client.Dto;
using CloudDataProtection.Services.Onboarding.Messaging.Listener;
using CloudDataProtection.Services.Onboarding.Messaging.Publisher;
using CloudDataProtection.Services.Onboarding.Messaging.Publisher.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CloudDataProtection.Services.Onboarding
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CloudDataProtection OnboardingService", Version = "v1"});
            });
            
            services.AddScoped<IOnboardingRepository, OnboardingRepository>();
            services.AddScoped<IGoogleCredentialsRepository, GoogleCredentialsRepository>();
            services.AddScoped<IGoogleLoginTokenRepository, LoginTokenRepository>();

            services.AddLazy<IRpcClient<GetUserEmailInput, GetUserEmailOutput>, GetUserEmailRpcClient>();
            services.AddLazy<IMessagePublisher<GoogleAccountConnectedModel>, GoogleAccountConnectedMessagePublisher>();
            
            services.Configure<GoogleOAuthV2Options>(options => Configuration.GetSection("Google:OAuth2").Bind(options));
            
            services.AddSingleton<ITokenGenerator, OtpGenerator>();
            services.AddSingleton<IGoogleOAuthV2CredentialsProvider, GoogleOAuthV2EnvironmentCredentialsProvider>();
            
            services.AddLazy<OnboardingBusinessLogic>();
            
            services.AddLazy<IRpcClient<GetUserEmailInput, GetUserEmailOutput>, GetUserEmailRpcClient>();
            services.AddLazy<IMessagePublisher<UserDataDeletedModel>, UserDataDeletedMessagePublisher>();
            
            services.Configure<RabbitMqConfiguration>(options => Configuration.GetSection("RabbitMq").Bind(options));
            services.Configure<OnboardingOptions>(options => Configuration.GetSection("Google:Onboarding").Bind(options));

            services.AddHostedService<BackupConfigurationEnteredMessageListener>();
            services.AddHostedService<UserDeletedMessageListener>();

            services.AddEncryptedDbContext<IOnboardingDbContext, OnboardingDbContext>(Configuration, builder =>
            {
                builder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod();
                });
            });

            ConfigureAuthentication(services);

            services.AddAutoMapper(ConfigureMapper);
        }

        private void ConfigureMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<Entities.Onboarding, OnboardingResult>()
                .ForMember(m => m.LoginInfo, options => options.Ignore());

            config.CreateMap<GoogleLoginInfo, GoogleLoginInfoResult>();
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
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnboardingService");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(CorsPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}