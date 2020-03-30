namespace MemoryBook.Business.RelationshipMembership.Managers
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

    public class RelationshipMembershipCommandManager : IRelationshipMembershipCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public RelationshipMembershipCommandManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<Guid>> CreateRelationshipMembership(params RelationshipMembershipCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IList<RelationshipMembership> entities = models.Select(CreateEntity).ToList();

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task DeleteRelationshipMembership(Guid memoryBookUniverseId, params Guid[] relationshipMembershipIds)
        {
            if (relationshipMembershipIds == null || !relationshipMembershipIds.Any())
            {
                return;
            }

            var relationships = await dbContext.Set<RelationshipMembership>()
                .Where(x => x.Relationship.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => relationshipMembershipIds.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            this.dbContext.RemoveRange(relationships);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static RelationshipMembership CreateEntity(RelationshipMembershipCreateModel model)
        {
            return new RelationshipMembership
            {
                MemberId = model.MemberId,
                MemberRelationshipTypeId = model.MemberRelationshipTypeId,
                RelationshipId = model.RelationshipId,
                StartTime = model.StartDate,
                EndTime = model.EndDate
            };
        }
    }
}