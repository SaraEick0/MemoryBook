namespace MemoryBook.Business.Group.Managers
{
    using System;
    using System.Threading.Tasks;
    using Common.Models;
    using Models;

    public interface IGroupCommandManager
    {
        Task<MemoryBookResponseModel> CreateGroups(Guid memoryBookUniverseId, params GroupCreateModel[] models);

        Task UpdateGroups(Guid memoryBookUniverseId, params GroupUpdateModel[] models);

        Task DeleteGroups(Guid memoryBookUniverseId, params Guid[] groupIds);
    }
}