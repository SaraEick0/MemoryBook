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

            return memberships.ToDictionary(x => x.EntityId, x => x.RelationshipMemberships);
        }

        public async Task<IList<RelationshipMembershipReadModel>> GetRelationshipMembershipsForRelationshipAsync(Guid memoryBookUniverseId, Guid relationshipId)
        {
            var relationshipMemberships = await this.relationshipMembershipQueryManager
                .GetRelationshipMembershipsForRelationships(memoryBookUniverseId, new List<Guid> { relationshipId }).ConfigureAwait(false);

            return relationshipMemberships?.FirstOrDefault()?.RelationshipMemberships;
        }

        public async Task<Guid> CreateRelationshipMembershipAsync(
            Guid relationshipId,
            IList<CombinedRelationshipMemberCreateModel> relationshipMembers,
            DateTime? relationshipStartDate,
            DateTime? relationshipEndDate)
        {
            Contract.RequiresNotNullOrEmpty(relationshipMembers, nameof(relationshipMembers));

            IList<RelationshipMembershipCreateModel> relationshipMembershipCreateModels = relationshipMembers
                .Select(x => new RelationshipMembershipCreateModel
                {
                    StartDate = x.StartDate ?? relationshipStartDate,
                    EndDate = x.EndDate ?? relationshipEndDate,
                    MemberId = x.MemberId,
                    MemberRelationshipTypeId = x.MemberRelationshipTypeId,
                    RelationshipId = relationshipId
                }).ToList();

            await this.relationshipMembershipCommandManager
                .CreateRelationshipMembership(relationshipMembershipCreateModels.ToArray())
                .ConfigureAwait(false);

            return relationshipId;
        }

        public async Task UpdateRelationshipMembershipAsync(Guid memoryBookUniverseId, IList<RelationshipMembershipReadModel> relationshipMembers, DateTime? relationshipStartDate, DateTime? relationshipEndDate)
        {
            Contract.RequiresNotNullOrEmpty(relationshipMembers, nameof(relationshipMembers));

            IList<RelationshipMembershipUpdateModel> relationshipMembershipUpdateModels = relationshipMembers
                .Select(x => new RelationshipMembershipUpdateModel
                {
                    Id = x.Id,
                    StartDate = x.StartDate ?? relationshipStartDate,
                    EndDate = x.EndDate ?? relationshipEndDate,
                    MemberId = x.MemberId,
                    MemberRelationshipTypeId = x.MemberRelationshipTypeId,
                    RelationshipId = x.RelationshipId
                }).ToList();

            await this.relationshipMembershipCommandManager
                .UpdateRelationshipMembership(memoryBookUniverseId, relationshipMembershipUpdateModels.ToArray())
                .ConfigureAwait(false);
        }

        public async Task DeleteRelationshipMembershipAsync(Guid memoryBookUniverseId, IList<RelationshipMembershipReadModel> relationshipMembers)
        {
            Contract.RequiresNotNullOrEmpty(relationshipMembers, nameof(relationshipMembers));

            var relationshipMemberIds = relationshipMembers.Select(x => x.Id).ToArray();

            await this.relationshipMembershipCommandManager
                .DeleteRelationshipMembership(memoryBookUniverseId, relationshipMemberIds)
                .ConfigureAwait(false);
        }
    }
}