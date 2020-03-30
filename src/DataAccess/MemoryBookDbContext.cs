namespace MemoryBook.DataAccess
{
    using System.Collections.Generic;
    using Entities;
    using Entities.EntityBuilders;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    public class MemoryBookDbContext : DbContext
    {
        private const string Schema = "MemoryBook";

        public DbSet<MemoryBookUniverse> MemoryBookUniverses { get; set; }

        public MemoryBookDbContext(DbContextOptions<MemoryBookDbContext> options)
            : base(options)
        {
            RelationalOptionsExtension.Extract(options).WithMigrationsHistoryTableSchema(Schema);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(modelBuilder);
            serviceCollection
                .AddSingleton<IEntityBuilder, DetailAssociationEntityBuilder>()
                .AddSingleton<IEntityBuilder, DetailEntityBuilder>()
                .AddSingleton<IEntityBuilder, DetailTypeEntityBuilder>()
                .AddSingleton<IEntityBuilder, DetailAssociationEntityBuilder>()
                .AddSingleton<IEntityBuilder, DetailPermissionEntityBuilder>()
                .AddSingleton<IEntityBuilder, GroupEntityBuilder>()
                .AddSingleton<IEntityBuilder, GroupMembershipEntityBuilder>()
                .AddSingleton<IEntityBuilder, MemberEntityBuilder>()
                .AddSingleton<IEntityBuilder, MemoryBookUniverseEntityBuilder>()
                .AddSingleton<IEntityBuilder, RelationshipEntityBuilder>()
                .AddSingleton<IEntityBuilder, RelationshipMembershipEntityBuilder>()
                .AddSingleton<IEntityBuilder, RelationshipTypeEntityBuilder>()
                .AddSingleton<IEntityBuilder, EntityTypeEntityBuilder>();

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IEnumerable<IEntityBuilder> modelBuilders = serviceProvider.GetServices<IEntityBuilder>();

            foreach (IEntityBuilder builder in modelBuilders)
            {
                builder.Build();
            }
        }
    }
}