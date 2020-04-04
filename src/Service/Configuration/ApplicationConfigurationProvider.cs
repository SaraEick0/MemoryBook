namespace MemoryBook.Service.Configuration
{
    using Microsoft.Extensions.Configuration;

    public class ApplicationConfigurationProvider : IApplicationConfigurationProvider
    {
        public IConfiguration GetConfiguration(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configurationBuilder.AddEnvironmentVariables();

            if (args != null)
            {
                configurationBuilder.AddCommandLine(args);
            }

            return configurationBuilder.Build();
        }
    }
}