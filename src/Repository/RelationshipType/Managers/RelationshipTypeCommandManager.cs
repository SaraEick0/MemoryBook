namespace MemoryBook.Repository.RelationshipType.Managers
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

    public class RelationshipTypeCommandManager : IRelationshipTypeCommandManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public RelationshipTypeCommandManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task CreateRelationshipType(params RelationshipTypeCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            List<RelationshipType> entities = models.Select(CreateEntity).ToList();

            this.databaseContext.AddRange(entities);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateRelationshipType(params RelationshipTypeUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            Dictionary<Guid, RelationshipType> groupDictionary = databaseContext.Set<RelationshipType>()
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<RelationshipType> entitiesToUpdate = new List<RelationshipType>();
            foreach (RelationshipTypeUpdateModel model in models)
            {
                if (model == null || !groupDictionary.TryGetValue(model.Id, out RelationshipType entity))
                {
                    continue;
                }

                entitiesToUpdate.Add(UpdateEntity(entity, model));
            }

            this.databaseContext.AddRange(entitiesToUpdate);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteRelationshipType(params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            Dictionary<Guid, RelationshipType> groupDictionary = databaseContext.Set<RelationshipType>()
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<RelationshipType> entitiesToDelete = new List<RelationshipType>();
            foreach (Guid id in groupIds)
            {
                if (!groupDictionary.TryGetValue(id, out RelationshipType entity))
                {
                    continue;
                }

                entitiesToDelete.Add(entity);
            }

            this.databaseContext.RemoveRange(entitiesToDelete);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static RelationshipType CreateEntity(RelationshipTypeCreateModel model)
        {
            return new RelationshipType
            {
                Code = model.Code
            };
        }

        private static RelationshipType UpdateEntity(RelationshipType entity, RelationshipTypeUpdateModel model)
        {
            entity.Code = model.Code;

            return entity;
        }
    }
}