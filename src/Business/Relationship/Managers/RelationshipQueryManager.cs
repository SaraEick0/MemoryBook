namespace MemoryBook.Business.Relationship.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Extensions;
    using MemoryBook.Common.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class RelationshipQueryManager : IRelationshipQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public RelationshipQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<RelationshipReadModel>> GetAllRelationships(Guid memoryBookUniverseId)
        {
            return await dbContext.Set<Relationship>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .Include(x => x.RelationshipMemberships)
                .ThenInclude(x => x.MemberRelationshipType)
                .Include(x => x.RelationshipMemberships)
                .ThenInclude(x => x.Member)
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<RelationshipReadModel>> GetRelationships(Guid memoryBookUniverseId, params Guid[] relationshipIds)
        {
            if (relationshipIds == null || relationshipIds.Length == 0)
            {
                return new List<RelationshipReadModel>();
            }

            return await dbContext.Set<Relationship>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => relationshipIds.Contains(x.Id))
                .Include(x => x.RelationshipMemberships)
                .ThenInclude(x => x.MemberRelationshipType)
                .Include(x => x.RelationshipMemberships)
                .ThenInclude(x => x.Member)
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}