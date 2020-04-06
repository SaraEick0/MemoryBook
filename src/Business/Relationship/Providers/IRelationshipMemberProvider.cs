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

        Task<Guid> CreateRelationshipMembershipAsync(
            Guid relationshipId,
            IList<RelationshipMemberModel> relationshipMembers,
            DateTime? startDate,
            DateTime? endDate);
    }
}