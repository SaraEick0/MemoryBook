namespace MemoryBook.Repository.RelationshipMembership.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRelationshipMembershipQueryManager
    {
        Task<IList<RelationshipMembershipReadModel>> GetAllRelationshipMemberships(Guid memoryBookUniverseId);

        Task<IList<RelationshipMembershipByEntityModel>> GetRelationshipMembershipsForMembers(Guid memoryBookUniverseId, IList<Guid> memberIds);

        Task<IList<RelationshipMembershipByEntityModel>> GetRelationshipMembershipsForRelationships(Guid memoryBookUniverseId, IList<Guid> relationshipIds);
    }
}