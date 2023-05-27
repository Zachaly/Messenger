using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Tests.Integration.Controller
{
    public class MessengerApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices((IServiceCollection services) =>
            {
                services.ConfigureRunner(c =>
                {
                    c.WithGlobalConnectionString("Server=localhost;Database=MessengerTest;Trusted_Connection=True;");
                });
            });
        }
    }
}
