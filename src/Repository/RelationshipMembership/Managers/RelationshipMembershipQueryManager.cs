namespace MemoryBook.Repository.RelationshipMembership.Managers
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

    public class RelationshipMembershipQueryManager : IRelationshipMembershipQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public RelationshipMembershipQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<RelationshipMembershipReadModel>> GetAllRelationshipMemberships(Guid memoryBookUniverseId)
        {
            return await dbContext.Set<RelationshipMembership>()
                .Where(x => x.Relationship.MemoryBookUniverseId == memoryBookUniverseId)
                .Include(x => x.Relationship)
                .Include(x => x.Member)
                .Include(x => x.MemberRelationshipType)
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<RelationshipMembershipReadModel>> GetRelationshipMembershipsForMembers(Guid memoryBookUniverseId, IList<Guid> memberIds)
        {
            return await dbContext.Set<RelationshipMembership>()
                .Where(x => x.Relationship.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => memberIds.Contains(x.MemberId))
                .Include(x => x.Relationship)
                .Include(x => x.Member)
                .Include(x => x.MemberRelationshipType)
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}