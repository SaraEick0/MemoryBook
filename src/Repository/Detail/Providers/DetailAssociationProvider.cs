namespace MemoryBook.Repository.Detail.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.Detail.Models;
    using Business.DetailAssociation.Managers;
    using Business.DetailAssociation.Models;
    using Business.EntityType;
    using Business.EntityType.Managers;
    using Business.EntityType.Models;
    using Business.Member.Models;
    using Business.Relationship.Models;
    using DataAccess.Interfaces;
    using MemoryBook.Business.Group.Models;
    using Microsoft.Extensions.Caching.Memory;

    public class DetailAssociationProvider : IDetailAssociationProvider
    {
        private readonly IEntityTypeQueryManager entityTypeQueryManager;
        private readonly IDetailAssociationCommandManager detailAssociationCommandManager;
        private readonly IDetailAssociationQueryManager detailAssociationQueryManager;
        private readonly IMemoryCache memoryCache;

        public DetailAssociationProvider(
            IEntityTypeQueryManager entityTypeQueryManager,
            IDetailAssociationCommandManager detailAssociationCommandManager,
            IDetailAssociationQueryManager detailAssociationQueryManager,
            IMemoryCache memoryCache)
        {
            this.entityTypeQueryManager = entityTypeQueryManager ?? throw new ArgumentNullException(nameof(entityTypeQueryManager));
            this.detailAssociationCommandManager = detailAssociationCommandManager ?? throw new ArgumentNullException(nameof(detailAssociationCommandManager));
            this.detailAssociationQueryManager = detailAssociationQueryManager ?? throw new ArgumentNullException(nameof(detailAssociationQueryManager));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task CreateDetailAssociation(DetailReadModel detail, IList<MemberReadModel> members)
        {
            await this.CreateDetailAssociation(detail, EntityTypeEnum.Member, members?.ToArray())
                .ConfigureAwait(false);
        }

        public async Task CreateDetailAssociation(DetailReadModel detail, RelationshipReadModel relationship)
        {
            await this.CreateDetailAssociation(detail, EntityTypeEnum.Relationship, relationship)
                .ConfigureAwait(false);
        }

        public async Task CreateDetailAssociation(DetailReadModel detail, GroupReadModel group)
        {
            await this.CreateDetailAssociation(detail, EntityTypeEnum.Group, group)
                .ConfigureAwait(false);
        }

        private async Task CreateDetailAssociation<T>(DetailReadModel detail, EntityTypeEnum entityTypeEnum, params T[] items)
            where T : class, IHasIdProperty
        {
            if (detail == null)
            {
                return;
            }
            if (items == null || items.Length == 0)
            {
                return;
            }

            var detailAssociations = await this.detailAssociationQueryManager.GetDetailAssociationByDetailId(detail.Id)
                .ConfigureAwait(false);

            var entityType = await this.GetEntityType(entityTypeEnum).ConfigureAwait(false);

            IList<DetailAssociationCreateModel> detailAssociationsToCreate = new List<DetailAssociationCreateModel>();
            foreach (var item in items)
            {
                if (detailAssociations != null && detailAssociations.Any(x => x.EntityId == item.Id))
                {
                    continue;
                }

                detailAssociationsToCreate.Add(new DetailAssociationCreateModel
                {
                    DetailId = detail.Id,
                    EntityTypeId = entityType.Id,
                    EntityId = item.Id
                });
            }

            await this.detailAssociationCommandManager.CreateDetailAssociation(detailAssociationsToCreate.ToArray())
                .ConfigureAwait(false);
        }

        private async Task<EntityTypeReadModel> GetEntityType(EntityTypeEnum entityType)
        {
            var types = await this.GetEntityTypes().ConfigureAwait(false);

            return types.First(x => x.Code.Equals(entityType.ToString()));
        }

        private async Task<IList<EntityTypeReadModel>> GetEntityTypes()
        {
            const string CacheKey = "EntityTypes";

            if (this.memoryCache.TryGetValue(CacheKey, out IList<EntityTypeReadModel> entityTypes))
            {
                return entityTypes;
            }

            entityTypes = await this.entityTypeQueryManager.GetAllEntityTypes().ConfigureAwait(false);

            this.memoryCache.Set(CacheKey, entityTypes);

            return entityTypes;
        }
    }
}