namespace MemoryBook.Business.Member.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities;
    using MemoryBook.Common;
    using MemoryBook.Common.Extensions;
    using MemoryBook.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MemberCommandManager : IMemberCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public MemberCommandManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<Guid>> CreateMembers(Guid memoryBookUniverseId, params MemberCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IList<Member> entities = models.Select(model => CreateEntity(memoryBookUniverseId, model)).ToList();

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task UpdateMembers(Guid memoryBookUniverseId, params MemberUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            var groupDictionary = dbContext.Set<Member>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<Member> entitiesToUpdate = new List<Member>();
            foreach (var model in models)
            {
                if (model == null || !groupDictionary.TryGetValue(model.Id, out var entity))
                {
                    continue;
                }

                entitiesToUpdate.Add(UpdateEntity(entity, model));
            }

            this.dbContext.AddRange(entitiesToUpdate);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteMembers(Guid memoryBookUniverseId, params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            var groupDictionary = dbContext.Set<Member>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<Member> entitiesToDelete = new List<Member>();
            foreach (var id in groupIds)
            {
                if (!groupDictionary.TryGetValue(id, out var entity))
                {
                    continue;
                }

                entitiesToDelete.Add(entity);
            }

            this.dbContext.RemoveRange(entitiesToDelete);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static Member CreateEntity(Guid memoryBookUniverseId, MemberCreateModel model)
        {
            return new Member
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                CommonName = model.CommonName,
                MemoryBookUniverseId = memoryBookUniverseId
            };
        }

        private static Member UpdateEntity(Member entity, MemberUpdateModel model)
        {
            entity.FirstName = model.FirstName;
            entity.MiddleName = model.MiddleName;
            entity.LastName = model.LastName;
            entity.CommonName = model.CommonName;

            return entity;
        }
    }
}