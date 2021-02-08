using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Services;

[assembly: FunctionsStartup(typeof(PelotonDadsChallenge.Startup))]
namespace PelotonDadsChallenge
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("local.settings.json", true)
                   .AddJsonFile("appsettings.json", true)
                   .AddEnvironmentVariables()
                   .Build();

            builder.Services.Configure<PelotonOptions>(config.GetSection("Peloton"));
            builder.Services.Configure<SendGridOptions>(config.GetSection("SendGrid"));
            builder.Services.AddScoped<IPelotonAuthenticationService, PelotonAuthenticationService>();
            builder.Services.AddScoped<IPelotonFollowersService, PelotonFollowersService>();
            builder.Services.AddScoped<IPelotonWorkoutsService, PelotonWorkoutsService>();
            builder.Services.AddScoped<IPelotonWorkoutService, PelotonWorkoutService>();
            builder.Services.AddScoped<ISendGridService, SendGridService>();

            builder.Services.AddOptions();
        }
    }
}
