namespace MemoryBook.Business.GroupMembership.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GroupMembershipCommandManager : IGroupMembershipCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public GroupMembershipCommandManager(MemoryBookDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<Guid>> CreateGroupMembership(Guid memoryBookUniverseId, params GroupMembershipCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IEnumerable<GroupMembership> entities = models.Select(model => CreateEntity(memoryBookUniverseId, model));

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task DeleteGroupMemberships(Guid memoryBookUniverseId, params Guid[] groupMembershipIds)
        {
            if (groupMembershipIds == null || !groupMembershipIds.Any())
            {
                return;
            }

            var groupMemberships = await dbContext.Set<GroupMembership>()
                .Where(x => x.Group.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => groupMembershipIds.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            this.dbContext.RemoveRange(groupMemberships);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
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