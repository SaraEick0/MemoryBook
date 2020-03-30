namespace Play
{
    using MemoryBook.Business.Detail.Managers;
    using MemoryBook.Business.DetailAssociation.Managers;
    using MemoryBook.Business.DetailType.Managers;
    using MemoryBook.Business.EntityType.Managers;
    using MemoryBook.Business.Group.Managers;
    using MemoryBook.Business.GroupMembership.Managers;
    using MemoryBook.Business.Member.Managers;
    using MemoryBook.Business.MemoryBookUniverse.Managers;
    using MemoryBook.Business.Relationship.Managers;
    using MemoryBook.Business.RelationshipMembership.Managers;
    using MemoryBook.Business.RelationshipType.Managers;
    using MemoryBook.Repository;
    using MemoryBook.Repository.Detail.Managers;
    using MemoryBook.Repository.Detail.Providers;
    using MemoryBook.Repository.Group.Managers;
    using MemoryBook.Repository.Member.Managers;
    using MemoryBook.Repository.MemoryBookUniverse.Managers;
    using MemoryBook.Repository.Relationship.Managers;
    using MemoryBook.Repository.SeedData;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this ServiceCollection services)
        {
            services.AddTransient<IGroupManager, GroupManager>();
            services.AddTransient<IDetailManager, DetailManager>();
            services.AddTransient<IMemberManager, MemberManager>();
            services.AddTransient<IMemoryBookUniverseManager, MemoryBookUniverseManager>();
            services.AddTransient<IRelationshipManager, RelationshipManager>();
            
            services.AddTransient<IDetailAssociationProvider, DetailAssociationProvider>();
            services.AddTransient<IDetailProvider, DetailProvider>();

            services.AddTransient<ISeedDataManager, SeedDataManager>();

            services
                .AddTransient<IGroupCommandManager, GroupCommandManager>()
                .AddTransient<IGroupQueryManager, GroupQueryManager>()
                .AddTransient<IGroupMembershipCommandManager, GroupMembershipCommandManager>()
                .AddTransient<IRelationshipTypeQueryManager, RelationshipTypeQueryManager>()
                .AddTransient<IRelationshipTypeCommandManager, RelationshipTypeCommandManager>()
                .AddTransient<IRelationshipCommandManager, RelationshipCommandManager>()
                .AddTransient<IRelationshipMembershipCommandManager, RelationshipMembershipCommandManager>()
                .AddTransient<IMemberCommandManager, MemberCommandManager>()
                .AddTransient<IMemberQueryManager, MemberQueryManager>()
                .AddTransient<IDetailTypeCommandManager, DetailTypeCommandManager>()
                .AddTransient<IDetailTypeQueryManager, DetailTypeQueryManager>()
                .AddTransient<IDetailCommandManager, DetailCommandManager>()
                .AddTransient<IDetailQueryManager, DetailQueryManager>()
                .AddTransient<IDetailAssociationCommandManager, DetailAssociationCommandManager>()
                .AddTransient<IDetailAssociationQueryManager, DetailAssociationQueryManager>()
                .AddTransient<IEntityTypeCommandManager, EntityTypeCommandManager>()
                .AddTransient<IEntityTypeQueryManager, EntityTypeQueryManager>()
                .AddTransient<IMemoryBookUniverseCommandManager, MemoryBookUniverseCommandManager>()
                .AddTransient<IMemoryBookUniverseQueryManager, MemoryBookUniverseQueryManager>();
            
            services.AddSingleton<Form1>();

            services.AddMemoryCache();
        }
    }
}