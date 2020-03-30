namespace MemoryBook.Business.RelationshipMembership.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRelationshipMembershipQueryManager
    {
        Task<IList<RelationshipMembershipReadModel>> GetAllRelationshipMemberships(Guid memoryBookUniverseId);

        Task<IList<RelationshipMembershipReadModel>> GetRelationshipMembershipsForMembers(Guid memoryBookUniverseId, IList<Guid> memberIds);
    }
}