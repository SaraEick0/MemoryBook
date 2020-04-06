namespace MemoryBook.Repository.Group.Managers
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

    public class GroupQueryManager : IGroupQueryManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public GroupQueryManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<GroupReadModel>> GetAllGroups(Guid memoryBookUniverseId)
        {
            List<Group> groups = await this.GetBaseQuery(memoryBookUniverseId)
                .ToListAsync();

            return await this.BuildModels(memoryBookUniverseId, groups);
        }

        public async Task<IList<GroupReadModel>> GetGroups(Guid memoryBookUniverseId, params Guid[] groupIds)
        {
            if (groupIds == null || groupIds.Length == 0)
            {
                return new List<GroupReadModel>();
            }

            List<Group> groups = await this.GetBaseQuery(memoryBookUniverseId)
                .Where(x => groupIds.Contains(x.Id))
                .ToListAsync();

            return await this.BuildModels(memoryBookUniverseId, groups);
        }

        private async Task<IList<GroupReadModel>> BuildModels(Guid memoryBookUniverseId, IList<Group> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return new List<GroupReadModel>();
            }

            ILookup<Guid, Guid> detailLookup = await this.databaseContext.GetDetailLookup(memoryBookUniverseId, EntityTypeEnum.Group).ConfigureAwait(false);

            return entities.Select(x => x.ToReadModel(detailLookup[x.Id]?.ToList())).ToList();
        }

        private IQueryable<Group> GetBaseQuery(Guid memoryBookUniverseId)
        {
            return this.databaseContext.Set<Group>()
                .AsNoTracking()
                .Include(x => x.GroupMemberships)
                .ThenInclude(x => x.Member)
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId);
        }
    }
}