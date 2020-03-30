namespace MemoryBook.Business.RelationshipType.Managers
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
        private readonly MemoryBookDbContext dbContext;

        public RelationshipTypeCommandManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task CreateRelationshipType(params RelationshipTypeCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            List<RelationshipType> entities = models.Select(CreateEntity).ToList();

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateRelationshipType(params RelationshipTypeUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            Dictionary<Guid, RelationshipType> groupDictionary = dbContext.Set<RelationshipType>()
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

            this.dbContext.AddRange(entitiesToUpdate);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteRelationshipType(params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            Dictionary<Guid, RelationshipType> groupDictionary = dbContext.Set<RelationshipType>()
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

            this.dbContext.RemoveRange(entitiesToDelete);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
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