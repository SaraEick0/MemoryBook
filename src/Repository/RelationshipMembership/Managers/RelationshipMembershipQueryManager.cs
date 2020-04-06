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
        private readonly MemoryBookDbContext databaseContext;

        public RelationshipMembershipQueryManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<RelationshipMembershipReadModel>> GetAllRelationshipMemberships(Guid memoryBookUniverseId)
        {
            return await databaseContext.Set<RelationshipMembership>()
                .Where(x => x.Relationship.MemoryBookUniverseId == memoryBookUniverseId)
                .Include(x => x.Relationship)
                .Include(x => x.Member)
                .Include(x => x.MemberRelationshipType)
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<RelationshipMembershipByMemberModel>> GetRelationshipMembershipsForMembers(Guid memoryBookUniverseId, IList<Guid> memberIds)
        {
            var relationshipMemberships =  await databaseContext.Set<RelationshipMembership>()
                .Where(x => x.Relationship.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => memberIds.Contains(x.MemberId))
                .Include(x => x.Relationship)
                .Include(x => x.Member)
                .Include(x => x.MemberRelationshipType)
                .AsNoTracking()
                .ToListAsync();

            return relationshipMemberships.GroupBy(x => x.MemberId)
                .Select(x => new RelationshipMembershipByMemberModel
                {
                    MemberId = x.Key,
                    RelationshipMemberships = x.Select(m => m.ToReadModel()).ToList()
                }).ToList();
        }
    }
}