namespace MemoryBook.Business.Member.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IMemberQueryManager
    {
        Task<IList<MemberReadModel>> GetAllMembers(Guid memoryBookUniverseId);

        Task<IList<MemberReadModel>> GetMembers(Guid memoryBookUniverseId, IList<Guid> memberIds);
    }
}