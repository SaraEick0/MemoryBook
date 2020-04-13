namespace MemoryBook.Business.Relationship.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Extensions;
    using Models;
    using Repository.Relationship.Managers;
    using Repository.Relationship.Models;

    public class RelationshipProvider : IRelationshipProvider
    {
        private readonly IRelationshipCommandManager relationshipCommandManager;
        private readonly IRelationshipQueryManager relationshipQueryManager;

        public RelationshipProvider(
            IRelationshipCommandManager relationshipCommandManager,
            IRelationshipQueryManager relationshipQueryManager)
        {
            Contract.RequiresNotNull(relationshipCommandManager, nameof(relationshipCommandManager));
            Contract.RequiresNotNull(relationshipQueryManager, nameof(relationshipQueryManager));

            this.relationshipCommandManager = relationshipCommandManager;
            this.relationshipQueryManager = relationshipQueryManager;
        }

        public async Task<IList<RelationshipReadModel>> GetRelationships(Guid memoryBookUniverseId, IList<Guid> relationshipIds)
        {
            Contract.RequiresNotNullOrEmpty(relationshipIds, nameof(relationshipIds));

            var relationships = await this.relationshipQueryManager.GetRelationships(memoryBookUniverseId, relationshipIds.ToArray());

            if (relationships.Count != relationshipIds.Count)
            {
                var invalid = relationshipIds.Where(x => relationships.All(y => y.Id != x)).ToList();
                throw new DataNotFoundException($"No relationships found for relationship ids: {string.Join(",", invalid)}", nameof(relationshipIds));
            }

            return relationships;
        }

        public async Task<Guid> CreateRelationship(
            Guid memoryBookUniverseId,
            DateTime? startDate,
            DateTime? endDate)
        {
            RelationshipCreateModel relationship = new RelationshipCreateModel
            {
                StartDate = startDate,
                EndDate = endDate
            };

            IList<Guid> relationshipIds = await this.relationshipCommandManager.CreateRelationships(memoryBookUniverseId, relationship)
                .ConfigureAwait(false);

            var relationshipId = relationshipIds.FirstOrDefault();

            return relationshipId;
        }

        public async Task UpdateRelationship(
            Guid memoryBookUniverseId,
            Guid relationshipId,
            DateTime? startDate,
            DateTime? endDate)
        {
            RelationshipUpdateModel relationship = new RelationshipUpdateModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Id = relationshipId
            };

            await this.relationshipCommandManager.UpdateRelationships(memoryBookUniverseId, relationship)
                .ConfigureAwait(false);
        }

        public async Task DeleteRelationships(Guid memoryBookUniverseId, params Guid[] relationshipIds)
        {
            await this.relationshipCommandManager.DeleteRelationships(memoryBookUniverseId, relationshipIds)
                .ConfigureAwait(false);
        }
    }
}