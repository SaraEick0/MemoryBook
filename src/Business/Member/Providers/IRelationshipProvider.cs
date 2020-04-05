namespace MemoryBook.Business.Member.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Repository.Relationship.Models;

    public interface IRelationshipProvider
    {
        Task<IList<RelationshipReadModel>> GetRelationships(Guid memoryBookUniverseId, IList<Guid> relationshipIds);

        Task<Guid> CreateRelationship(
            IList<RelationshipMemberModel> relationshipMembers,
            DateTime? startDate,
            DateTime? endDate);
    }
}