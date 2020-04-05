namespace MemoryBook.Repository.Member.Managers
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

    public class MemberQueryManager : IMemberQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public MemberQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<MemberReadModel>> GetAllMembers(Guid memoryBookUniverseId)
        {
            return await dbContext.Set<Member>()
                .AsNoTracking()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .Include(x => x.RelationshipMemberships)
                .ThenInclude(x => x.Relationship)
                .Include(x => x.CreatedDetails)
                .Include(x => x.GroupMemberships)
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<MemberReadModel>> GetMembers(Guid memoryBookUniverseId, IList<Guid> memberIds)
        {
            if (memberIds == null || memberIds.Count == 0)
            {
                return new List<MemberReadModel>();
            }

            return await dbContext.Set<Member>()
                .AsNoTracking()
                .Include(x => x.RelationshipMemberships)
                .ThenInclude(x => x.Relationship)
                .Include(x => x.CreatedDetails)
                .Include(x => x.GroupMemberships)
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => memberIds.Contains(x.Id))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}