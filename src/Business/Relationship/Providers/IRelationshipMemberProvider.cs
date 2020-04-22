namespace MemoryBook.Business.Relationship.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Repository.RelationshipMembership.Models;

    public interface IRelationshipMemberProvider
    {
        Task<IDictionary<Guid, IList<RelationshipMembershipReadModel>>> GetRelationshipMembershipsForMembersAsync(
            Guid memoryBookUniverseId,
            IList<Guid> memberIds);

        Task<IList<RelationshipMembershipReadModel>> GetRelationshipMembershipsForRelationshipAsync(
            Guid memoryBookUniverseId,
            Guid relationshipId);

        Task<Guid> CreateRelationshipMembershipAsync(
            Guid relationshipId,
            IList<CombinedRelationshipMemberCreateModel> relationshipMembers,
            DateTime? relationshipStartDate,
            DateTime? relationshipEndDate);

        Task UpdateRelationshipMembershipAsync(
            Guid memoryBookUniverseId,
            IList<RelationshipMembershipReadModel> relationshipMembers,
            DateTime? relationshipStartDate,
            DateTime? relationshipEndDate);

        Task DeleteRelationshipMembershipAsync(
            Guid memoryBookUniverseId,
            IList<RelationshipMembershipReadModel> relationshipMembers);
    }
}