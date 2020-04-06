namespace MemoryBook.Business.DataCoordinators.Managers
{
    using System;
    using System.Threading.Tasks;
    using Group.Models;

    public interface IViewCoordinator
    {
        Task<GroupViewModel> GetGroupViewModel(Guid memoryBookUniverseId, Guid groupId);
    }
}