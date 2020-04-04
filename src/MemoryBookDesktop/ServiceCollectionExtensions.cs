namespace MemoryBook.Desktop
{
    using Business.Detail.Managers;
    using Business.DetailAssociation.Managers;
    using Business.DetailType.Managers;
    using Business.EntityType.Managers;
    using Business.Group.Managers;
    using Business.GroupMembership.Managers;
    using Business.Member.Managers;
    using Business.MemoryBookUniverse.Managers;
    using Business.Relationship.Managers;
    using Business.RelationshipMembership.Managers;
    using Business.RelationshipType.Managers;
    using Microsoft.Extensions.DependencyInjection;
    using Repository.Detail.Managers;
    using Repository.Detail.Providers;
    using Repository.Group.Managers;
    using Repository.Member.Managers;
    using Repository.Member.Providers;
    using Repository.MemoryBookUniverse.Managers;
    using Repository.SeedData;

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
                .AddTransient<IMemoryBookUniverseQueryManager, MemoryBookUniverseQueryManager>();
            
            services.AddSingleton<MemoryBook>();

            services.AddMemoryCache();
            services.AddLogging();
        }
    }
}