namespace MemoryBook.Business.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities;
    using MemoryBook.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GroupCommandManager : IGroupCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public GroupCommandManager(MemoryBookDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<Guid>> CreateGroups(Guid memoryBookUniverseId, params GroupCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IEnumerable<Group> entities = models.Select(x => new Group
            {
                MemoryBookUniverseId = memoryBookUniverseId,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description
            });

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task UpdateGroups(Guid memoryBookUniverseId, params GroupUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            var groupDictionary = dbContext.Set<Group>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<Group> entitiesToUpdate = new List<Group>();
            foreach (var model in models)
            {
                if (model == null || !groupDictionary.TryGetValue(model.Id, out var entity))
                {
                    continue;
                }

                entity.Name = model.Name;
                entity.Code = model.Code;
                entity.Description = model.Description;

                entitiesToUpdate.Add(entity);
            }

            this.dbContext.AddRange(entitiesToUpdate);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteGroups(Guid memoryBookUniverseId, params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            var groupDictionary = dbContext.Set<Group>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<Group> entitiesToDelete = new List<Group>();
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
    }
}