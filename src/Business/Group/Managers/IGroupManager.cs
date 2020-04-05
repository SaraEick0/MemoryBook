namespace MemoryBook.Business.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repository.Group.Models;
    using Repository.Member.Models;

    public interface IGroupManager
    {
        Task<GroupReadModel> CreateGroup(Guid memoryBookUniverseId, string code, string name, string description);

        Task AddMembersToGroup(Guid memoryBookUniverseId, GroupReadModel group, IList<MemberReadModel> members);
    }
}