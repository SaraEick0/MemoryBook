namespace MemoryBook.Desktop
{
    using MemoryBook.Business.Detail.Managers;
    using MemoryBook.Business.Detail.Providers;
    using MemoryBook.Business.Group.Managers;
    using MemoryBook.Business.Member.Managers;
    using MemoryBook.Business.Member.Providers;
    using MemoryBook.Business.MemoryBookUniverse.Managers;
    using MemoryBook.Business.SeedData;
    using Microsoft.Extensions.DependencyInjection;
    using Repository.Detail.Managers;
    using Repository.DetailAssociation.Managers;
    using Repository.DetailType.Managers;
    using Repository.EntityType.Managers;
    using Repository.Group.Managers;
    using Repository.GroupMembership.Managers;
    using Repository.Member.Managers;
    using Repository.MemoryBookUniverse.Managers;
    using Repository.Relationship.Managers;
    using Repository.RelationshipMembership.Managers;
    using Repository.RelationshipType.Managers;

    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this ServiceCollection services)
        {
            services.AddTransient<IGroupManager, GroupManager>();
            services.AddTransient<IRelationshipDetailManager, RelationshipDetailManager>();
            services.AddTransient<IMemberDetailManager, MemberDetailManager>();
            services.AddTransient<IMemberManager, MemberManager>();
            services.AddTransient<IMemoryBookUniverseManager, MemoryBookUniverseManager>();
            
            services.AddTransient<IDetailAssociationProvider, DetailAssociationProvider>();
            services.AddTransient<IDetailProvider, DetailProvider>();
            services.AddTransient<IMemberProvider, MemberProvider>();
            services.AddTransient<IRelationshipTypeProvider, RelationshipTypeProvider>();
            services.AddTransient<IRelationshipProvider, RelationshipProvider>();

            services.AddTransient<ISeedDataManager, SeedDataManager>();

            services
                .AddTransient<IGroupCommandManager, GroupCommandManager>()
                .AddTransient<IGroupQueryManager, GroupQueryManager>()
                .AddTransient<IGroupMembershipCommandManager, GroupMembershipCommandManager>()
                .AddTransient<IRelationshipTypeQueryManager, RelationshipTypeQueryManager>()
                .AddTransient<IRelationshipTypeCommandManager, RelationshipTypeCommandManager>()
                .AddTransient<IRelationshipCommandManager, RelationshipCommandManager>()
                .AddTransient<IRelationshipQueryManager, RelationshipQueryManager>()
                .AddTransient<IRelationshipMembershipCommandManager, RelationshipMembershipCommandManager>()
                .AddTransient<IRelationshipMembershipQueryManager, RelationshipMembershipQueryManager>()
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
                .AddTransient<IMemoryBookUniverseQueryManager, MemoryBookUniverseQueryManager>()
                .AddTransient<IGroupViewCoordinator, GroupViewCoordinator>();

            services.AddSingleton<MemoryBookForm>();

            services.AddMemoryCache();
            services.AddLogging();
        }
    }
}