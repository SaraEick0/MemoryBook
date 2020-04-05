namespace MemoryBook.Repository.Relationship.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRelationshipCommandManager
    {
        Task<IList<Guid>> CreateRelationship(Guid memoryBookUniverseId, params RelationshipCreateModel[] models);

        Task DeleteRelationships(Guid memoryBookUniverseId, params Guid[] relationshipIds);
    }
}