namespace MemoryBook.StorageInitializer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
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
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.applicationConfiguration = applicationConfiguration.Value ?? throw new ArgumentNullException(nameof(applicationConfiguration));
            this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        }

        private ApplicationOptions ApplicationOptions => this.applicationConfiguration.Application;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogDebug("Business Rule Database Migration background task execution starting.");

            stoppingToken.Register(() => this.logger.LogDebug($"Business Rule Database Migration background task is stopping."));

            if (!stoppingToken.IsCancellationRequested)
            {
                this.logger.LogInformation("Starting database migration...");

                this.databaseContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
                this.databaseContext.Database.Migrate();

                this.logger.LogInformation("Database migration completed successfully.");
            }

            await Task.FromResult(Task.CompletedTask).ConfigureAwait(false);

            this.logger.LogDebug("Business Rule Database Migration background task execution completed successfully.");
        }
    }
}