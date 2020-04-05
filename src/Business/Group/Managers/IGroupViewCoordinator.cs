namespace MemoryBook.Business.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repository.Group.Models;

    public interface IGroupViewCoordinator
    {
        Task<IList<GroupReadModel>> GetAllGroupsAsync(Guid memoryBookUniverseId);
    }
}