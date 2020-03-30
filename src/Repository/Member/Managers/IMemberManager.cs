namespace MemoryBook.Repository.Member.Managers
{
    using System;
    using System.Threading.Tasks;
    using Business.Member.Models;

    public interface IMemberManager
    {
        Task<MemberReadModel> CreateMember(Guid memoryBookUniverseId, string firstName, string middleName, string lastName, string commonName);
    }
}