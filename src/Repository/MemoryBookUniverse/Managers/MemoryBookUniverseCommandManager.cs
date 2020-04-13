namespace MemoryBook.Repository.MemoryBookUniverse.Managers
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

    public class MemoryBookUniverseCommandManager : IMemoryBookUniverseCommandManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public MemoryBookUniverseCommandManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<Guid>> CreateMemoryBookUniverse(params MemoryBookUniverseCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            List<MemoryBookUniverse> entities = models.Select(CreateEntity).ToList();

            this.databaseContext.AddRange(entities);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task UpdateMemoryBookUniverse(params MemoryBookUniverseUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            IList<Guid> memoryBookIds = models.Select(x => x.Id).ToList();
            
            var memoryBooks = await this.databaseContext.Set<MemoryBookUniverse>()
                .Where(x => memoryBookIds.Contains(x.Id))
                .ToListAsync()
                .ConfigureAwait(false);

            IList<MemoryBookUniverse> memoryBookUniversesToUpdate = new List<MemoryBookUniverse>();
            foreach (var model in models)
            {
                var entity = memoryBooks.FirstOrDefault(x => x.Id == model.Id);

                if (entity == null)
                {
                    continue;
                }

                memoryBookUniversesToUpdate.Add(UpdateEntity(entity, model));
            }

            this.databaseContext.UpdateRange(memoryBookUniversesToUpdate);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteMemoryBookUniverse(params Guid[] memoryBookUniverseIds)
        {
            if (memoryBookUniverseIds == null || !memoryBookUniverseIds.Any())
            {
                return;
            }

            Dictionary<Guid, MemoryBookUniverse> universeDictionary = databaseContext.Set<MemoryBookUniverse>()
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<MemoryBookUniverse> entitiesToDelete = new List<MemoryBookUniverse>();
            foreach (Guid id in memoryBookUniverseIds)
            {
                if (!universeDictionary.TryGetValue(id, out MemoryBookUniverse entity))
                {
                    continue;
                }

                entitiesToDelete.Add(entity);
            }

            this.databaseContext.RemoveRange(entitiesToDelete);
            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static MemoryBookUniverse CreateEntity(MemoryBookUniverseCreateModel model)
        {
            Contract.RequiresNotNull(model, nameof(model));

            return new MemoryBookUniverse
            {
                Name = model.Name,
                CreatedDate = DateTime.UtcNow
            };
        }

        private static MemoryBookUniverse UpdateEntity(MemoryBookUniverse entity, MemoryBookUniverseUpdateModel model)
        {
            Contract.RequiresNotNull(entity, nameof(entity));
            Contract.RequiresNotNull(model, nameof(model));

            entity.Name = model.Name;

            return entity;
        }
    }
}