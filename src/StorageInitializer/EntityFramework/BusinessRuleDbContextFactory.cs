namespace MemoryBook.StorageInitializer.EntityFramework
{
    using System.Reflection;
    using Configuration;
    using DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public sealed class MemoryBookDbContextFactory : IDesignTimeDbContextFactory<MemoryBookDbContext>
    {
        private readonly ApplicationOptions applicationOptions;

        public MemoryBookDbContextFactory()
        {
            this.applicationOptions = CreateConfiguration().GetSection("Application").Get<ApplicationOptions>();
        }

        public MemoryBookDbContext CreateDbContext(params string[] args)
        {
            DbContextOptionsBuilder<MemoryBookDbContext> databaseContextOptionsBuilder = new DbContextOptionsBuilder<MemoryBookDbContext>()
                .UseSqlServer(
                    this.applicationOptions.ConnectionString,
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));

            return new MemoryBookDbContext(databaseContextOptionsBuilder.Options);
        }

        private static IConfigurationRoot CreateConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }
    }
}