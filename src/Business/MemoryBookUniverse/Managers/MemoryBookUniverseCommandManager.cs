namespace MemoryBook.Business.MemoryBookUniverse.Managers
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
        private readonly MemoryBookDbContext dbContext;

        public MemoryBookUniverseCommandManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<Guid>> CreateMemoryBookUniverse(params MemoryBookUniverseCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            List<MemoryBookUniverse> entities = models.Select(CreateEntity).ToList();

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task DeleteMemoryBookUniverse(params Guid[] memoryBookUniverseIds)
        {
            if (memoryBookUniverseIds == null || !memoryBookUniverseIds.Any())
            {
                return;
            }

            foreach (var entry in this.dbContext.ChangeTracker.Entries().ToList())
            {
                this.dbContext.Entry(entry.Entity).State = EntityState.Detached;
            }

            Dictionary<Guid, MemoryBookUniverse> universeDictionary = dbContext.Set<MemoryBookUniverse>()
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<MemoryBookUniverse> entitiesToDelete = new List<MemoryBookUniverse>();
            foreach (Guid id in memoryBookUniverseIds)
            {
                if (!universeDictionary.TryGetValue(id, out MemoryBookUniverse entity))
                {
                    continue;
                }

                this.dbContext.Remove(entity);
            }

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
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