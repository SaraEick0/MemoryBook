namespace MemoryBook.StorageInitializer.EntityFramework
{
    using System.Reflection;
    using Configuration;
    using DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, ApplicationOptions applicationOptions)
        {
            DbContextOptionsBuilder<MemoryBookDbContext> databaseContextOptionsBuilder = new DbContextOptionsBuilder<MemoryBookDbContext>()
                .UseSqlServer(
                    applicationOptions.ConnectionString,
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));

            services.AddSingleton(x => databaseContextOptionsBuilder.Options);

            services.AddDbContext<MemoryBookDbContext>(ServiceLifetime.Scoped);

            return services;
        }
    }
}