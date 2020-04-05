namespace MemoryBook.Business.Member.Managers
{
    using System;
    using System.Threading.Tasks;
    using Repository.Member.Models;
    using Repository.RelationshipType;

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