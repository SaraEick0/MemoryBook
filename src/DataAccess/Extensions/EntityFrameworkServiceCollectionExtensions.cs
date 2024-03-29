﻿namespace MemoryBook.DataAccess.Extensions
{
    using DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
    using Microsoft.Extensions.DependencyInjection;

    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseContexts(this IServiceCollection services, string connectionString, ServiceLifetime serviceLifetime)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MemoryBookDbContext>();

            // add connection string
            RelationalOptionsExtension extension = (optionsBuilder.Options.FindExtension<SqlServerOptionsExtension>() ?? new SqlServerOptionsExtension()).WithConnectionString(connectionString);
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            var coreOptionsExtension = optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();

            coreOptionsExtension = coreOptionsExtension.WithWarningsConfiguration(
                coreOptionsExtension.WarningsConfiguration.TryWithExplicit(RelationalEventId.AmbientTransactionWarning, WarningBehavior.Throw));
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);

            services.AddSingleton(x => optionsBuilder
                    .Options);

            services.AddDbContext<MemoryBookDbContext>(serviceLifetime);

            return services;
        }
    }
}