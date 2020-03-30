namespace MemoryBook.Repository.Member.Providers
{
    using System;
    using System.Threading.Tasks;
    using Business.Member.Models;

    public interface IMemberProvider
    {
        Task<MemberReadModel> CreateMember(Guid memoryBookUniverseId, string firstName, string middleName, string lastName, string commonName);
    }
}