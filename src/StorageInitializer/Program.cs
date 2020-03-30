namespace MemoryBook.StorageInitializer
{
    using System.Threading.Tasks;
    using Configuration;
    using EntityFramework;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class Program
    {
        private static void Main(string[] args)
        {
            Task.Run(() => MainAsync(args)).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            IHostBuilder builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.AddJsonFile("appsettings.json", optional: false);
                    configuration.AddEnvironmentVariables();

                    if (args != null)
                    {
                        configuration.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();

                    IConfiguration configuration = hostContext.Configuration;

                    ApplicationConfiguration applicationConfiguration = new ApplicationConfiguration();
                    configuration.Bind(applicationConfiguration);

                    services.AddOptions();
                    services.Configure<ApplicationConfiguration>(configuration);

                    services.AddDatabaseContext(applicationConfiguration.Application);

                    services.AddHostedService<MemoryBookDatabaseMigrationService>();
                });

            using (IHost host = builder.Build())
            {
                await host.StartAsync().ConfigureAwait(false);

                await host.StopAsync().ConfigureAwait(false);
            }
        }
    }
}