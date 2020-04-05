namespace MemoryBook.Business.Member.Providers
{
    using System;
    using System.Threading.Tasks;
    using Repository.Member.Models;

    public interface IMemberProvider
    {
        Task<MemberReadModel> CreateMember(Guid memoryBookUniverseId, string firstName, string middleName, string lastName, string commonName);
    }
}