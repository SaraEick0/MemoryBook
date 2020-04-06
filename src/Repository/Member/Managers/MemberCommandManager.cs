namespace MemoryBook.Repository.Member.Managers
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

    public class MemberCommandManager : IMemberCommandManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public MemberCommandManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<Guid>> CreateMembers(Guid memoryBookUniverseId, params MemberCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IList<Member> entities = models.Select(model => CreateEntity(memoryBookUniverseId, model)).ToList();

            this.databaseContext.AddRange(entities);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task UpdateMembers(Guid memoryBookUniverseId, params MemberUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            var groupDictionary = databaseContext.Set<Member>()
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

            this.databaseContext.AddRange(entitiesToUpdate);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteMembers(Guid memoryBookUniverseId, params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            var groupDictionary = databaseContext.Set<Member>()
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

            this.databaseContext.RemoveRange(entitiesToDelete);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
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