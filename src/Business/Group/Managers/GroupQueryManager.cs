namespace MemoryBook.Business.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities;
    using Extensions;
    using MemoryBook.Common.Extensions;
    using MemoryBook.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GroupQueryManager : IGroupQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public GroupQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<GroupReadModel>> GetAllGroups(Guid memoryBookUniverseId)
        {
            return await dbContext.Set<Group>()
                .AsNoTracking()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .Include(x => x.GroupMemberships)
                .ThenInclude(x => x.Member)
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<GroupReadModel>> GetGroups(Guid memoryBookUniverseId, IList<Guid> memberIds)
        {
            if (memberIds == null || memberIds.Count == 0)
            {
                return new List<GroupReadModel>();
            }

            return await dbContext.Set<Group>()
                .AsNoTracking()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => memberIds.Contains(x.Id))
                .Include(x => x.GroupMemberships)
                .ThenInclude(x => x.Member)
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}