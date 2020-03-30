namespace MemoryBook.Repository.Member.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Business.Relationship.Models;
    using Models;

    public interface IRelationshipProvider
    {
        Task<IList<RelationshipReadModel>> GetRelationships(Guid memoryBookUniverseId, IList<Guid> relationshipIds);

        Task<Guid> CreateRelationship(
            IList<RelationshipMemberModel> relationshipMembers,
            DateTime? startDate,
            DateTime? endDate);
    }
}