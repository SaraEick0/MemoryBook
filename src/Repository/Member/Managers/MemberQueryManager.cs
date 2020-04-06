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
    using MemoryBook.Repository.Detail.Extensions;
    using MemoryBook.Repository.EntityType;
    using MemoryBook.Repository.Member.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MemberQueryManager : IMemberQueryManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public MemberQueryManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<MemberReadModel>> GetAllMembers(Guid memoryBookUniverseId)
        {
            var entities = await this.GetBaseQuery(memoryBookUniverseId)
                .ToListAsync();

            return await this.BuildModels(memoryBookUniverseId, entities).ConfigureAwait(false);
        }

        public async Task<IList<MemberReadModel>> GetMembers(Guid memoryBookUniverseId, IList<Guid> memberIds)
        {
            if (memberIds == null || memberIds.Count == 0)
            {
                return new List<MemberReadModel>();
            }

            var entities = await this.GetBaseQuery(memoryBookUniverseId)
                .Where(x => memberIds.Contains(x.Id))
                .ToListAsync();

            return await this.BuildModels(memoryBookUniverseId, entities).ConfigureAwait(false);
        }

        public async Task<IList<MemberReadModel>> GetMembersByGroup(Guid memoryBookUniverseId, Guid groupId)
        {
            List<Member> entities = await this.GetBaseQuery(memoryBookUniverseId)
                .Where(x => x.GroupMemberships != null && x.GroupMemberships.Any(r => r.GroupId == groupId))
                .ToListAsync();

            return await this.BuildModels(memoryBookUniverseId, entities).ConfigureAwait(false);
        }

        private async Task<IList<MemberReadModel>> BuildModels(Guid memoryBookUniverseId, IList<Member> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return new List<MemberReadModel>();
            }

            ILookup<Guid, Guid> detailLookup = await this.databaseContext.GetDetailLookup(memoryBookUniverseId, EntityTypeEnum.Member).ConfigureAwait(false);

            return entities.Select(x => x.ToReadModel(detailLookup[x.Id]?.ToList())).ToList();
        }

        private IQueryable<Member> GetBaseQuery(Guid memoryBookUniverseId)
        {
            return this.databaseContext.Set<Member>()
                .AsNoTracking()
                .Include(x => x.RelationshipMemberships)
                .ThenInclude(x => x.Relationship)
                .Include(x => x.CreatedDetails)
                .Include(x => x.GroupMemberships)
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId);
        }
    }
}