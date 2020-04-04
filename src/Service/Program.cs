namespace MemoryBook.Service
{
    using System;
    using Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using WebHost;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                bool enableConsoleLogging = false;
                IApplicationConfigurationProvider applicationConfigurationProvider = new ApplicationConfigurationProvider();
                IConfiguration configuration = applicationConfigurationProvider.GetConfiguration(args);
                
#if DEBUG
                enableConsoleLogging = true;
#endif
                ApplicationOptions applicationOptions = configuration.GetSection(ApplicationOptions.ConfigurationKey).Get<ApplicationOptions>();

                ServiceWebHostBuilderOptions webHostBuilderOptions = new ServiceWebHostBuilderOptions
                {
                    Configuration = configuration,
                    Options = applicationOptions,
                    EnableConsoleLogging = enableConsoleLogging,
                };

                webHostBuilderOptions.SetArguments(args);

                IServiceWebHostBuilder serviceWebHostBuilder = new ServiceWebHostBuilder();
                serviceWebHostBuilder.Build(webHostBuilderOptions).Run();
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                Console.Error.WriteLine(message);
            }
        }
    }
}