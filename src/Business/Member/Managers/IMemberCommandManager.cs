namespace MemoryBook.Business.Member.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IMemberCommandManager
    {
        Task<IList<Guid>> CreateMembers(Guid memoryBookUniverseId, params MemberCreateModel[] models);

        Task UpdateMembers(Guid memoryBookUniverseId, params MemberUpdateModel[] models);

        Task DeleteMembers(Guid memoryBookUniverseId, params Guid[] groupIds);
    }
}