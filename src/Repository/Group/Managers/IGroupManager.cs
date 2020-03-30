namespace MemoryBook.Repository.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Business.Group.Models;
    using Business.Member.Models;

    public interface IGroupManager
    {
        Task<GroupReadModel> CreateGroup(Guid memoryBookUniverseId, string code, string name, string description);

        Task AddMembersToGroup(Guid memoryBookUniverseId, GroupReadModel group, IList<MemberReadModel> members);
    }
}