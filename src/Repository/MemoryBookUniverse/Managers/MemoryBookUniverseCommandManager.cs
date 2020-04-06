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

        public async Task DeleteMemoryBookUniverse(params Guid[] memoryBookUniverseIds)
        {
            if (memoryBookUniverseIds == null || !memoryBookUniverseIds.Any())
            {
                return;
            }

            foreach (var entry in this.databaseContext.ChangeTracker.Entries().ToList())
            {
                this.databaseContext.Entry(entry.Entity).State = EntityState.Detached;
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

                this.databaseContext.Remove(entity);
            }

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static MemoryBookUniverse CreateEntity(MemoryBookUniverseCreateModel model)
        {
            return new MemoryBookUniverse
            {
                Name = model.Name,
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}