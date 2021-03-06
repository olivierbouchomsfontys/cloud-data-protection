using CloudDataProtection.Services.MailService.Business;
using CloudDataProtection.Services.MailService.Messaging.Listener;
using CloudDataProtection.Services.MailService.Sender;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.SendGrid;
using CloudDataProtection.Services.MailService.SendGrid.Credentials;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudDataProtection.Services.MailService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitMqConfiguration>(options => Configuration.GetSection("RabbitMq").Bind(options));
            
            services.AddHostedService<UserRegisteredMessageListener>();
            services.AddHostedService<GoogleAccountConnectedMessageListener>();
            services.AddHostedService<UserDeletionCompleteMessageListener>();
            services.AddHostedService<EmailChangeRequestedMessageListener>();

            services.AddSingleton<IMailSender, SendGridMailSender>();
            
            services.AddSingleton<RegistrationMailLogic>();
            services.AddSingleton<AccountMailLogic>();
            
            services.AddSingleton<ISendGridCredentialsProvider, SendGridEnvironmentCredentialsProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}