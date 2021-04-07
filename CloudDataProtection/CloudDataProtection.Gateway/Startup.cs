using System.Text;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Data;
using CloudDataProtection.Data.Context;
using CloudDataProtection.Jwt;
using CloudDataProtection.Messaging.Publisher;
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CloudDataProtection ApiGateway", Version = "v1"});
            });

            ConfigureAuthentication(services);
            
            services.AddLazy<UserRegisteredMessagePublisher>();
            
            services.Configure<RabbitMqConfiguration>(options => Configuration.GetSection("RabbitMq").Bind(options));

            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGateway v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            await app.UseOcelot();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddDbContext<IAuthenticationDbContext, AuthenticationDbContext>
                (o => o.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            
            // TODO Use Azure Key Vault
            byte[] key = Encoding.ASCII.GetBytes("jwtSecretButNowLonger");

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
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<AuthenticationBusinessLogic>();
            
            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            services.AddScoped<ITokenValidatedHandler, TokenValidatedHandler>();
        }

        private async Task OnTokenValidated(TokenValidatedContext context)
        {
            ITokenValidatedHandler handler = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatedHandler>();

            await handler.Handle(context);
        }
    }
}