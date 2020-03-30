namespace MemoryBook.Business.Detail.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities;
    using MemoryBook.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DetailCommandManager : IDetailCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public DetailCommandManager(MemoryBookDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<Guid>> CreateDetails(Guid memoryBookUniverseId, params DetailCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return null;
            }

            List<Detail> entities = models.Select(x => CreateEntity(memoryBookUniverseId, x)).ToList();

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task UpdateDetails(Guid memoryBookUniverseId, params DetailUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            Dictionary<Guid, Detail> groupDictionary = dbContext.Set<Detail>()
                .Where(x => x.Creator.MemoryBookUniverseId == memoryBookUniverseId)
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<Detail> entitiesToUpdate = new List<Detail>();
            foreach (DetailUpdateModel model in models)
            {
                if (model == null || !groupDictionary.TryGetValue(model.Id, out Detail entity))
                {
                    continue;
                }

                entitiesToUpdate.Add(UpdateEntity(entity, model));
            }

            this.dbContext.AddRange(entitiesToUpdate);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteDetails(Guid memoryBookUniverseId, params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            Dictionary<Guid, Detail> groupDictionary = dbContext.Set<Detail>()
                .Where(x => x.Creator.MemoryBookUniverseId == memoryBookUniverseId)
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<Detail> entitiesToDelete = new List<Detail>();
            foreach (Guid id in groupIds)
            {
                if (!groupDictionary.TryGetValue(id, out Detail entity))
                {
                    continue;
                }

                entitiesToDelete.Add(entity);
            }

            this.dbContext.RemoveRange(entitiesToDelete);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static Detail CreateEntity(Guid memoryBookUniverseId, DetailCreateModel model)
        {
            return new Detail
            {
                CreatorId = model.CreatorId,
                CustomDetailText = model.CustomDetailText,
                DetailTypeId = model.DetailTypeId,
                Story = model.Story,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };
        }

        private static Detail UpdateEntity(Detail entity, DetailUpdateModel model)
        {
            entity.CustomDetailText = model.CustomDetailText;
            entity.DetailTypeId = model.DetailTypeId;
            entity.StartTime = model.StartTime;
            entity.EndTime = model.EndTime;
            entity.Story = model.Story;

            return entity;
        }
    }
}