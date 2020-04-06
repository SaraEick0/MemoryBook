namespace MemoryBook.Business.Group.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repository.Group.Models;
    using Repository.Member.Models;

    public interface IGroupProvider
    {
        Task<IList<GroupReadModel>> GetAllGroupsAsync(Guid memoryBookUniverseId);

        Task<IList<GroupReadModel>> GetGroupsAsync(Guid memoryBookUniverseId, params Guid[] groupIds);

        Task<Guid> CreateGroup(Guid memoryBookUniverseId, string code, string name, string description);

        Task AddMembersToGroup(Guid memoryBookUniverseId, Guid groupId, IList<MemberReadModel> members);
    }
}