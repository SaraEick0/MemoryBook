namespace MemoryBook.Business.RelationshipMembership.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRelationshipMembershipCommandManager
    {
        Task<IList<Guid>> CreateRelationshipMembership(params RelationshipMembershipCreateModel[] models);

        Task DeleteRelationshipMembership(Guid memoryBookUniverseId, params Guid[] relationshipMembershipIds);
    }
}