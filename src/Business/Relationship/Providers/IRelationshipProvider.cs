namespace MemoryBook.Business.Relationship.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repository.Relationship.Models;

    public interface IRelationshipProvider
    {
        Task<IList<RelationshipReadModel>> GetRelationships(Guid memoryBookUniverseId, IList<Guid> relationshipIds);

        Task<Guid> CreateRelationship(
            Guid memoryBookUniverseId,
            DateTime? startDate,
            DateTime? endDate);

        Task UpdateRelationship(
            Guid memoryBookUniverseId,
            Guid relationshipId,
            DateTime? startDate,
            DateTime? endDate);

        Task DeleteRelationships(
            Guid memoryBookUniverseId,
            params Guid[] relationshipIds);
    }
}