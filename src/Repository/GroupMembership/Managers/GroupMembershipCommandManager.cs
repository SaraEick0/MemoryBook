namespace MemoryBook.Repository.GroupMembership.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using MemoryBook.Common.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GroupMembershipCommandManager : IGroupMembershipCommandManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public GroupMembershipCommandManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<Guid>> CreateGroupMembership(Guid memoryBookUniverseId, params GroupMembershipCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IList<GroupMembership> entities = models.Select(model => CreateEntity(memoryBookUniverseId, model)).ToList();

            this.databaseContext.AddRange(entities);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task DeleteGroupMemberships(Guid memoryBookUniverseId, params Guid[] groupMembershipIds)
        {
            if (groupMembershipIds == null || !groupMembershipIds.Any())
            {
                return;
            }

            var groupMemberships = await databaseContext.Set<GroupMembership>()
                .Where(x => x.Group.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => groupMembershipIds.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            this.databaseContext.RemoveRange(groupMemberships);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static GroupMembership CreateEntity(Guid memoryBookUniverseId, GroupMembershipCreateModel model)
        {
            return new GroupMembership
            {
                GroupId = model.GroupId,
                MemberId = model.MemberId
            };
        }
    }
}