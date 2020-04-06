namespace MemoryBook.Business.Member.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repository.Member.Models;

    public interface IMemberProvider
    {
        Task<IList<MemberReadModel>> GetAllMembers(Guid memoryBookUniverseId);

        Task<IList<MemberReadModel>> GetMembers(Guid memoryBookUniverseId, IList<Guid> memberIds);

        Task<MemberReadModel> CreateMember(Guid memoryBookUniverseId, string firstName, string middleName, string lastName, string commonName);
    }
}