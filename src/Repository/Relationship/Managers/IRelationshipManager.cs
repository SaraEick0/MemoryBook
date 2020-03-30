namespace MemoryBook.Repository.Relationship.Managers
{
    using System;
    using System.Threading.Tasks;
    using Business.Member.Models;
    using Business.RelationshipType;

    public interface IRelationshipManager
    {
        Task CreateRelationship(MemberReadModel firstMember, MemberReadModel secondMember, RelationshipTypeEnum firstMemberRelationshipType, RelationshipTypeEnum secondMemberRelationshipType, DateTime? startDate, DateTime? endDate);
    }
}