namespace MemoryBook.Business.MemoryBookUniverse.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MemoryBookUniverseCommandManager : IMemoryBookUniverseCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public MemoryBookUniverseCommandManager(MemoryBookDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<Guid>> CreateMemoryBookUniverse(params MemoryBookUniverseCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            List<MemoryBookUniverse> entities = models.Select(x => CreateEntity(x)).ToList();

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task DeleteMemoryBookUniverse(params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            Dictionary<Guid, MemoryBookUniverse> groupDictionary = dbContext.Set<MemoryBookUniverse>()
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<MemoryBookUniverse> entitiesToDelete = new List<MemoryBookUniverse>();
            foreach (Guid id in groupIds)
            {
                if (!groupDictionary.TryGetValue(id, out MemoryBookUniverse entity))
                {
                    continue;
                }

                entitiesToDelete.Add(entity);
            }

            this.dbContext.RemoveRange(entitiesToDelete);

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