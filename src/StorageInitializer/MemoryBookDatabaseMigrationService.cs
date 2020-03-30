namespace MemoryBook.StorageInitializer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Configuration;
    using DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MemoryBookDatabaseMigrationService : BackgroundService, IHostedService
    {
        private readonly ILogger<MemoryBookDatabaseMigrationService> logger;
        private readonly ApplicationConfiguration applicationConfiguration;
        private readonly MemoryBookDbContext databaseContext;

        public MemoryBookDatabaseMigrationService(
            ILogger<MemoryBookDatabaseMigrationService> logger,
            IOptions<ApplicationConfiguration> applicationConfiguration,
            MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(logger, nameof(logger));
            Contract.RequiresNotNull(applicationConfiguration, nameof(applicationConfiguration));
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));

            this.logger = logger;
            this.applicationConfiguration = applicationConfiguration.Value;
            this.databaseContext = databaseContext;
        }

        private ApplicationOptions ApplicationOptions => this.applicationConfiguration.Application;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogDebug("Memory Book Database Migration background task execution starting.");

            stoppingToken.Register(() => this.logger.LogDebug("Memory Book Database Migration background task is stopping."));

            if (!stoppingToken.IsCancellationRequested)
            {
                this.logger.LogInformation("Starting database migration...");

                this.databaseContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
                this.databaseContext.Database.Migrate();

                this.logger.LogInformation("Database migration completed successfully.");
            }

            await Task.FromResult(Task.CompletedTask).ConfigureAwait(false);

            this.logger.LogDebug("Memory Book Database Migration background task execution completed successfully.");
        }
    }
}