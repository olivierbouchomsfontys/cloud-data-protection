using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Business.Options;
using CloudDataProtection.Core.Cryptography.Generator;
using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Jwt.Options;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Data;
using CloudDataProtection.Data.Context;
using CloudDataProtection.Dto.Result;
using CloudDataProtection.Email;
using CloudDataProtection.Jwt;
using CloudDataProtection.Messaging.Listener;
using CloudDataProtection.Messaging.Publisher;
using CloudDataProtection.Messaging.Server;
using CloudDataProtection.Ocelot;
using CloudDataProtection.Ocelot.Swagger;
using CloudDataProtection.Password;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace CloudDataProtection
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
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<OcelotControllerFilter>();
                
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CloudDataProtection ApiGateway", Version = "v1"});
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
            
            services.AddLazy<IMessagePublisher<UserResult>, UserRegisteredMessagePublisher>();
            services.AddLazy<IMessagePublisher<UserDeletedModel>, UserDeletedMessagePublisher>();
            services.AddLazy<IMessagePublisher<UserDeletionCompleteModel>, UserDeletionCompleteMessagePublisher>();
            services.AddLazy<IMessagePublisher<EmailChangeRequestedModel>, EmailChangeRequestedMessagePublisher>();
            
            services.AddSingleton<ITokenGenerator, OtpGenerator>();

            services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();

            services.AddTransient<UserBusinessLogic>();

            services.Configure<RabbitMqConfiguration>(options => Configuration.GetSection("RabbitMq").Bind(options));
            services.Configure<JwtSecretOptions>(options => Configuration.GetSection("Jwt").Bind(options));
            services.Configure<ChangeEmailOptions>(options => Configuration.GetSection("ChangeEmail").Bind(options));

            services.AddHostedService<GetUserEmailRpcServer>();
            services.AddHostedService<UserDataDeletedMessageListener>();
            
            services.AddOcelot()
                .AddDelegatingHandler<BackupDemoFileDownloadHandler>()
                .AddDelegatingHandler<BackupDemoFileUploadHandler>()
                .AddDelegatingHandler<BackupDemoFileInfoHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGateway v1"));

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(CorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            await app.UseOcelot();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddEncryptedDbContext<IAuthenticationDbContext, AuthenticationDbContext>
                (Configuration, o => o.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            JwtSecretOptions options = new JwtSecretOptions();
            
            Configuration.GetSection("Jwt").Bind(options);
            
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = OnTokenValidated
                    };
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
            
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<AuthenticationBusinessLogic>();
            
            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IJwtDecoder, JwtDecoder>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IEmailHasher, BCryptEmailHasher>();

            services.AddScoped<ITokenValidatedHandler, TokenValidatedHandler>();
        }

        private async Task OnTokenValidated(TokenValidatedContext context)
        {
            ITokenValidatedHandler handler = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatedHandler>();

            await handler.Handle(context);
        }
    }
}