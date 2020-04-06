namespace MemoryBook.Repository.Relationship.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Extensions;
    using MemoryBook.Common.Extensions;
    using MemoryBook.Repository.Detail.Extensions;
    using MemoryBook.Repository.EntityType;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class RelationshipQueryManager : IRelationshipQueryManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public RelationshipQueryManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<RelationshipReadModel>> GetAllRelationships(Guid memoryBookUniverseId)
        {
            var entities = await this.GetBaseQuery(memoryBookUniverseId)
                .ToListAsync();

            return await this.BuildModels(memoryBookUniverseId, entities).ConfigureAwait(false);
        }

        public async Task<IList<RelationshipReadModel>> GetRelationships(Guid memoryBookUniverseId, params Guid[] relationshipIds)
        {
            if (relationshipIds == null || relationshipIds.Length == 0)
            {
                return new List<RelationshipReadModel>();
            }

            var entities = await this.GetBaseQuery(memoryBookUniverseId)
                .Where(x => relationshipIds.Contains(x.Id))
                .ToListAsync();

            return await this.BuildModels(memoryBookUniverseId, entities).ConfigureAwait(false);
        }

        private async Task<IList<RelationshipReadModel>> BuildModels(Guid memoryBookUniverseId, IList<Relationship> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return new List<RelationshipReadModel>();
            }

            ILookup<Guid, Guid> detailLookup = await this.databaseContext.GetDetailLookup(memoryBookUniverseId, EntityTypeEnum.Relationship).ConfigureAwait(false);

            return entities.Select(x => x.ToReadModel(detailLookup[x.Id]?.ToList())).ToList();
        }

        private IQueryable<Relationship> GetBaseQuery(Guid memoryBookUniverseId)
        {
            return this.databaseContext.Set<Relationship>()
                .AsNoTracking()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .Include(x => x.RelationshipMemberships)
                .ThenInclude(x => x.MemberRelationshipType)
                .Include(x => x.RelationshipMemberships)
                .ThenInclude(x => x.Member);
        }
    }
}