namespace MemoryBook.Business.EntityType.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using MemoryBook.Common;
    using MemoryBook.Common.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class EntityTypeCommandManager : IEntityTypeCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public EntityTypeCommandManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task CreateEntityType(params EntityTypeCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            List<EntityType> entities = models.Select(CreateEntity).ToList();

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateEntityType(params EntityTypeUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            Dictionary<Guid, EntityType> groupDictionary = dbContext.Set<EntityType>()
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<EntityType> entitiesToUpdate = new List<EntityType>();
            foreach (EntityTypeUpdateModel model in models)
            {
                if (model == null || !groupDictionary.TryGetValue(model.Id, out EntityType entity))
                {
                    continue;
                }

                entitiesToUpdate.Add(UpdateEntity(entity, model));
            }

            this.dbContext.AddRange(entitiesToUpdate);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteEntityType(params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            Dictionary<Guid, EntityType> groupDictionary = dbContext.Set<EntityType>()
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<EntityType> entitiesToDelete = new List<EntityType>();
            foreach (Guid id in groupIds)
            {
                if (!groupDictionary.TryGetValue(id, out EntityType entity))
                {
                    continue;
                }

                entitiesToDelete.Add(entity);
            }

            this.dbContext.RemoveRange(entitiesToDelete);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static EntityType CreateEntity(EntityTypeCreateModel model)
        {
            return new EntityType
            {
                Code = model.Code
            };
        }

        private static EntityType UpdateEntity(EntityType entity, EntityTypeUpdateModel model)
        {
            entity.Code = model.Code;

            return entity;
        }
    }
}