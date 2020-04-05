namespace MemoryBook.Repository.Relationship.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRelationshipQueryManager
    {
        Task<IList<RelationshipReadModel>> GetAllRelationships(Guid memoryBookUniverseId);

        Task<IList<RelationshipReadModel>> GetRelationships(Guid memoryBookUniverseId, params Guid[] relationshipIds);
    }
}