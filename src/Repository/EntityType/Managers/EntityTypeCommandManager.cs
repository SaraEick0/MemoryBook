namespace MemoryBook.Repository.EntityType.Managers
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

    public class EntityTypeCommandManager : IEntityTypeCommandManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public EntityTypeCommandManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task CreateEntityType(params EntityTypeCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            List<EntityType> entities = models.Select(CreateEntity).ToList();

            this.databaseContext.AddRange(entities);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateEntityType(params EntityTypeUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            Dictionary<Guid, EntityType> groupDictionary = databaseContext.Set<EntityType>()
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

            this.databaseContext.AddRange(entitiesToUpdate);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteEntityType(params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            Dictionary<Guid, EntityType> groupDictionary = databaseContext.Set<EntityType>()
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

            this.databaseContext.RemoveRange(entitiesToDelete);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
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