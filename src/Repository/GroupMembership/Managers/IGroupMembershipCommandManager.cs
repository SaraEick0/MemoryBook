namespace MemoryBook.Repository.GroupMembership.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IGroupMembershipCommandManager
    {
        Task<IList<Guid>> CreateGroupMembership(Guid memoryBookUniverseId, params GroupMembershipCreateModel[] models);

        Task DeleteGroupMemberships(Guid memoryBookUniverseId, params Guid[] groupMembershipIds);
    }
}