namespace MemoryBook.Repository.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Business.Group.Models;

    public interface IGroupViewCoordinator
    {
        Task<IList<GroupReadModel>> GetAllGroupsAsync(Guid memoryBookUniverseId);
    }
}