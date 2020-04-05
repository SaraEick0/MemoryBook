namespace MemoryBook.Business.Member.Providers
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
    using Repository.RelationshipMembership.Managers;
    using Repository.RelationshipMembership.Models;

    public class RelationshipProvider : IRelationshipProvider
    {
        private readonly IRelationshipCommandManager relationshipCommandManager;
        private readonly IRelationshipQueryManager relationshipQueryManager;
        private readonly IRelationshipMembershipCommandManager relationshipMembershipCommandManager;

        public RelationshipProvider(
            IRelationshipCommandManager relationshipCommandManager,
            IRelationshipQueryManager relationshipQueryManager,
            IRelationshipMembershipCommandManager relationshipMembershipCommandManager)
        {
            Contract.RequiresNotNull(relationshipCommandManager, nameof(relationshipCommandManager));
            Contract.RequiresNotNull(relationshipQueryManager, nameof(relationshipQueryManager));
            Contract.RequiresNotNull(relationshipMembershipCommandManager, nameof(relationshipMembershipCommandManager));

            this.relationshipCommandManager = relationshipCommandManager;
            this.relationshipQueryManager = relationshipQueryManager;
            this.relationshipMembershipCommandManager = relationshipMembershipCommandManager;
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
            IList<RelationshipMemberModel> relationshipMembers,
            DateTime? startDate,
            DateTime? endDate)
        {
            Contract.RequiresNotNullOrEmpty(relationshipMembers, nameof(relationshipMembers));

            RelationshipCreateModel relationship = new RelationshipCreateModel
            {
                StartDate = startDate,
                EndDate = endDate
            };

            IList<Guid> relationshipIds = await this.relationshipCommandManager.CreateRelationship(relationshipMembers.First().Member.MemoryBookUniverseId, relationship)
                .ConfigureAwait(false);

            var relationshipId = relationshipIds.FirstOrDefault();

            IList<RelationshipMembershipCreateModel> relationshipMembershipCreateModels = relationshipMembers
                .Select(x => new RelationshipMembershipCreateModel
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    MemberId = x.Member.Id,
                    MemberRelationshipTypeId = x.RelationshipType.Id,
                    RelationshipId = relationshipId
                }).ToList();

            await this.relationshipMembershipCommandManager
                .CreateRelationshipMembership(relationshipMembershipCreateModels.ToArray())
                .ConfigureAwait(false);

            return relationshipId;
        }
    }
}