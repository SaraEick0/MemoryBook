namespace MemoryBook.Repository.RelationshipMembership.Managers
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

    public class RelationshipMembershipCommandManager : IRelationshipMembershipCommandManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public RelationshipMembershipCommandManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<Guid>> CreateRelationshipMembership(params RelationshipMembershipCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IList<RelationshipMembership> entities = models.Select(CreateEntity).ToList();

            this.databaseContext.AddRange(entities);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task UpdateRelationshipMembership(Guid memoryBookUniverseId, params RelationshipMembershipUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            var relationshipMembershipIds = models.Select(x => x.Id).ToList();

            var entities = await this.databaseContext.Set<RelationshipMembership>()
                .Where(x => x.Relationship.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => relationshipMembershipIds.Contains(x.Id))
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

        public async Task DeleteRelationshipMembership(Guid memoryBookUniverseId, params Guid[] relationshipMembershipIds)
        {
            if (relationshipMembershipIds == null || !relationshipMembershipIds.Any())
            {
                return;
            }

            var relationships = await databaseContext.Set<RelationshipMembership>()
                .Where(x => x.Relationship.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => relationshipMembershipIds.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            this.databaseContext.RemoveRange(relationships);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
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

        private static void UpdateEntity(RelationshipMembership entity, RelationshipMembershipUpdateModel model)
        {
            entity.MemberId = model.MemberId;
            entity.MemberRelationshipTypeId = model.MemberRelationshipTypeId;
            entity.RelationshipId = model.RelationshipId;
            entity.StartTime = model.StartDate;
            entity.EndTime = model.EndDate;
        }
    }
}