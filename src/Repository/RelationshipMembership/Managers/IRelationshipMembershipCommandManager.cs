namespace MemoryBook.Repository.RelationshipMembership.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRelationshipMembershipCommandManager
    {
        Task<IList<Guid>> CreateRelationshipMembership(params RelationshipMembershipCreateModel[] models);

        Task UpdateRelationshipMembership(Guid memoryBookUniverseId, params RelationshipMembershipUpdateModel[] models);

        Task DeleteRelationshipMembership(Guid memoryBookUniverseId, params Guid[] relationshipMembershipIds);
    }
}