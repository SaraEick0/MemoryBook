namespace MemoryBook.Business.Detail.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using DataAccess.Interfaces;
    using Microsoft.Extensions.Caching.Memory;
    using Repository.Detail.Models;
    using Repository.DetailAssociation.Managers;
    using Repository.DetailAssociation.Models;
    using Repository.EntityType;
    using Repository.EntityType.Managers;
    using Repository.EntityType.Models;
    using Repository.Group.Models;
    using Repository.Member.Models;
    using Repository.Relationship.Models;

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
            Contract.RequiresNotNull(entityTypeQueryManager, nameof(entityTypeQueryManager));
            Contract.RequiresNotNull(detailAssociationCommandManager, nameof(detailAssociationCommandManager));
            Contract.RequiresNotNull(detailAssociationQueryManager, nameof(detailAssociationQueryManager));
            Contract.RequiresNotNull(memoryCache, nameof(memoryCache));

            this.entityTypeQueryManager = entityTypeQueryManager;
            this.detailAssociationCommandManager = detailAssociationCommandManager;
            this.detailAssociationQueryManager = detailAssociationQueryManager;
            this.memoryCache = memoryCache;
        }

        public async Task CreateMemberDetailAssociation(DetailReadModel detail, IList<Guid> memberIds)
        {
            await this.CreateDetailAssociation(detail, EntityTypeEnum.Member, memberIds?.ToArray())
                .ConfigureAwait(false);
        }

        public async Task CreateRelationshipDetailAssociation(DetailReadModel detail, Guid relationshipId)
        {
            await this.CreateDetailAssociation(detail, EntityTypeEnum.Relationship, relationshipId)
                .ConfigureAwait(false);
        }

        public async Task CreateGroupDetailAssociation(DetailReadModel detail, Guid groupId)
        {
            await this.CreateDetailAssociation(detail, EntityTypeEnum.Group, groupId)
                .ConfigureAwait(false);
        }

        private async Task CreateDetailAssociation(DetailReadModel detail, EntityTypeEnum entityTypeEnum, params Guid[] entityIds)
        {
            if (detail == null)
            {
                return;
            }

            if (entityIds == null || entityIds.Length == 0)
            {
                return;
            }

            var detailAssociations = await this.detailAssociationQueryManager.GetDetailAssociationByDetailId(detail.Id)
                .ConfigureAwait(false);

            var entityType = await this.GetEntityType(entityTypeEnum).ConfigureAwait(false);

            IList<DetailAssociationCreateModel> detailAssociationsToCreate = new List<DetailAssociationCreateModel>();
            foreach (var id in entityIds)
            {
                if (detailAssociations != null && detailAssociations.Any(x => x.EntityId == id))
                {
                    continue;
                }

                detailAssociationsToCreate.Add(new DetailAssociationCreateModel
                {
                    DetailId = detail.Id,
                    EntityTypeId = entityType.Id,
                    EntityId = id
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