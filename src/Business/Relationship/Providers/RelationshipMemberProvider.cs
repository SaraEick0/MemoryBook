namespace MemoryBook.Business.Relationship.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Models;
    using Repository.RelationshipMembership.Managers;
    using Repository.RelationshipMembership.Models;

    public class RelationshipMemberProvider : IRelationshipMemberProvider
    {
        private readonly IRelationshipMembershipQueryManager relationshipMembershipQueryManager;
        private readonly IRelationshipMembershipCommandManager relationshipMembershipCommandManager;

        public RelationshipMemberProvider(
            IRelationshipMembershipQueryManager relationshipMembershipQueryManager,
            IRelationshipMembershipCommandManager relationshipMembershipCommandManager)
        {
            Contract.RequiresNotNull(relationshipMembershipQueryManager, nameof(relationshipMembershipQueryManager));
            Contract.RequiresNotNull(relationshipMembershipCommandManager, nameof(relationshipMembershipCommandManager));

            this.relationshipMembershipQueryManager = relationshipMembershipQueryManager;
            this.relationshipMembershipCommandManager = relationshipMembershipCommandManager;
        }

        public async Task<IDictionary<Guid, IList<RelationshipMembershipReadModel>>> GetRelationshipMembershipsForMembersAsync(
            Guid memoryBookUniverseId,
            IList<Guid> memberIds)
        {
            var memberships = await this.relationshipMembershipQueryManager
                .GetRelationshipMembershipsForMembers(memoryBookUniverseId, memberIds).ConfigureAwait(false);

            return memberships.ToDictionary(x => x.MemberId, x => x.RelationshipMemberships);
        }

        public async Task<Guid> CreateRelationshipMembershipAsync(
            Guid relationshipId,
            IList<RelationshipMemberModel> relationshipMembers,
            DateTime? startDate,
            DateTime? endDate)
        {
            Contract.RequiresNotNullOrEmpty(relationshipMembers, nameof(relationshipMembers));

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