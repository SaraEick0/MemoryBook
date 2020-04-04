namespace MemoryBook.Service.WebHost
{
    using System;
    using Common.Extensions;
    using Configuration;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Serilog.Events;
    using Serilog.Formatting.Compact;

    public class ServiceWebHostBuilder : IServiceWebHostBuilder
    {
        public IWebHost Build(ServiceWebHostBuilderOptions webHostBuilderOptions)
        {
            Contract.RequiresNotNull(webHostBuilderOptions, nameof(webHostBuilderOptions));

            var configuration = webHostBuilderOptions.Configuration;
            var hostOptions = webHostBuilderOptions.Options;
            var enableConsoleLogging = webHostBuilderOptions.EnableConsoleLogging;
            var arguments = webHostBuilderOptions.GetArguments();

            IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder(arguments)
                .UseStartup<Startup>()
                .UseSerilog((Action<WebHostBuilderContext, LoggerConfiguration>)((hostingContext, loggerConfiguration) =>
                {
                    string infoLogName = "MemoryBook-{Date}.json";
                    string errorLogName = "MemoryBook-Errors-{Date}.log";
                    if (!string.IsNullOrWhiteSpace(hostOptions.LogFolder))
                    {
                        string logFolder = hostOptions.LogFolder + "\\";
                        infoLogName = logFolder + infoLogName;
                        errorLogName = logFolder + errorLogName;
                    }

                    if (enableConsoleLogging)
                    {
                        LoggerConfigurationLiterateExtensions.LiterateConsole(loggerConfiguration.WriteTo,
                            LogEventLevel.Verbose,
                            "{Timestamp:o} [{Level:u3}] {SourceContext:l} {Scope} {Application} {Message}{NewLine}{Exception}",
                            (IFormatProvider)null, new LogEventLevel?());
                    }

                    RollingFileLoggerConfigurationExtensions.RollingFile(RollingFileLoggerConfigurationExtensions.RollingFile(
                            loggerConfiguration.WriteTo,
                            new CompactJsonFormatter(),
                            infoLogName,
                            LogEventLevel.Verbose,
                            1073741824L,
                            5,
                            null,
                            false,
                            false,
                            new TimeSpan?())
                            .WriteTo,
                            errorLogName,
                            LogEventLevel.Error,
                            "{Timestamp:o} [{Level:u3}] {SourceContext:l} {Scope} ({Application}/{MachineName}/{ThreadId}) {Message}{NewLine}{Exception}",
                            null,
                            1073741824L,
                            31,
                            null,
                            false,
                            false,
                            new TimeSpan?())
                        .Enrich.WithProperty("Application", "Memory Book")
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithThreadId()
                        .Enrich.WithEnvironmentUserName();
                }))
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<ApplicationConfiguration>(configuration);
                })
                .UseUrls(hostOptions.BindUrl);

            return webHostBuilder.Build();
        }
    }
}