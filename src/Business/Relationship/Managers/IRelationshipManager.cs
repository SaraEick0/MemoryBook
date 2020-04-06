namespace MemoryBook.Business.Relationship.Managers
{
    using System;
    using System.Threading.Tasks;
    using Repository.Member.Models;
    using Repository.RelationshipType;

    public interface IRelationshipManager
    {
        Task<Guid> CreateRelationship(
            MemberReadModel firstMember,
            MemberReadModel secondMember,
            RelationshipTypeEnum firstMemberRelationshipType,
            RelationshipTypeEnum secondMemberRelationshipType,
            DateTime? startDate,
            DateTime? endDate);
    }
}