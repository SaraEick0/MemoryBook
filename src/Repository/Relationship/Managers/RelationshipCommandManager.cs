namespace MemoryBook.Repository.Relationship.Managers
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

    public class RelationshipCommandManager : IRelationshipCommandManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public RelationshipCommandManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<Guid>> CreateRelationships(Guid memoryBookUniverseId, params RelationshipCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IEnumerable<Relationship> entities = models.Select(model => CreateEntity(memoryBookUniverseId, model)).ToList();

            this.databaseContext.AddRange(entities);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task UpdateRelationships(Guid memoryBookUniverseId, params RelationshipUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            var relationshipIds = models.Select(x => x.Id).ToList();

            var entities = await this.databaseContext.Set<Relationship>()
                .Where(x => relationshipIds.Contains(x.Id))
                .ToListAsync();

            foreach (var model in models)
            {
                var entity = entities.FirstOrDefault(x => x.Id == model.Id);

                if (entity == null)
                {
                    continue;
                }

                UpdateEntity(entity, model);
            }

            this.databaseContext.UpdateRange(entities);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteRelationships(Guid memoryBookUniverseId, params Guid[] relationshipIds)
        {
            if (relationshipIds == null || !relationshipIds.Any())
            {
                return;
            }

            var relationships = await databaseContext.Set<Relationship>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => relationshipIds.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            this.databaseContext.RemoveRange(relationships);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static Relationship CreateEntity(Guid memoryBookUniverseId, RelationshipCreateModel model)
        {
            return new Relationship
            {
                StartTime = model.StartDate,
                EndTime = model.EndDate,
                MemoryBookUniverseId = memoryBookUniverseId
            };
        }

        private static void UpdateEntity(Relationship relationship, RelationshipUpdateModel model)
        {
            relationship.StartTime = model.StartDate;
            relationship.EndTime = model.EndDate;
        }
    }
}