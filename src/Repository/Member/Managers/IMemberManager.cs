namespace MemoryBook.Repository.Member.Managers
{
    using System;
    using System.Threading.Tasks;
    using Business.Member.Models;
    using Business.RelationshipType;

    public interface IMemberManager
    {
        Task<MemberReadModel> CreateMember(Guid memoryBookUniverseId, string firstName, string middleName, string lastName, string commonName);

        Task CreateRelationship(
            Guid memoryBookUniverseId,
            MemberReadModel firstMember,
            MemberReadModel secondMember,
            RelationshipTypeEnum firstMemberRelationshipType,
            RelationshipTypeEnum secondMemberRelationshipType,
            DateTime? startDate,
            DateTime? endDate);
    }
}