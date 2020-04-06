namespace MemoryBook.Desktop
{
    using Business.DataCoordinators.Managers;
    using Business.Group.Providers;
    using Business.Relationship.Managers;
    using Business.Relationship.Providers;
    using MemoryBook.Business.Detail.Managers;
    using MemoryBook.Business.Detail.Providers;
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
            services.AddTransient<IGroupProvider, GroupProvider>();
            services.AddTransient<IRelationshipDetailManager, RelationshipDetailManager>();
            services.AddTransient<IMemberDetailManager, MemberDetailManager>();
            services.AddTransient<IRelationshipManager, RelationshipManager>();
            services.AddTransient<Business.Member.Managers.IMemberManager, MemberManager>();
            services.AddTransient<IMemoryBookUniverseManager, MemoryBookUniverseManager>();
            
            services.AddTransient<IDetailAssociationProvider, DetailAssociationProvider>();
            services.AddTransient<IDetailProvider, DetailProvider>();
            services.AddTransient<Business.Member.Providers.IMemberProvider, MemberProvider>();
            services.AddTransient<IRelationshipTypeProvider, RelationshipTypeProvider>();
            services.AddTransient<IRelationshipProvider, RelationshipProvider>();
            services.AddTransient<IRelationshipMemberProvider, RelationshipMemberProvider>();

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
                .AddTransient<IViewCoordinator, ViewCoordinator>();

            services.AddSingleton<MemoryBookForm>();

            services.AddMemoryCache();
            services.AddLogging();
        }
    }
}