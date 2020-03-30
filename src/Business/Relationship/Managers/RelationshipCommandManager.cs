namespace MemoryBook.Business.Relationship.Managers
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

    public class RelationshipCommandManager : IRelationshipCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public RelationshipCommandManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<Guid>> CreateRelationship(Guid memoryBookUniverseId, params RelationshipCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IEnumerable<Relationship> entities = models.Select(model => CreateEntity(memoryBookUniverseId, model)).ToList();

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task DeleteRelationships(Guid memoryBookUniverseId, params Guid[] relationshipIds)
        {
            if (relationshipIds == null || !relationshipIds.Any())
            {
                return;
            }

            var relationships = await dbContext.Set<Relationship>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => relationshipIds.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            this.dbContext.RemoveRange(relationships);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
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
    }
}