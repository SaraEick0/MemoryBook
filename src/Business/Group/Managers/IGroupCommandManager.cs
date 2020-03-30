namespace MemoryBook.Business.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IGroupCommandManager
    {
        Task<IList<Guid>> CreateGroups(Guid memoryBookUniverseId, params GroupCreateModel[] models);

        Task UpdateGroups(Guid memoryBookUniverseId, params GroupUpdateModel[] models);

        Task DeleteGroups(Guid memoryBookUniverseId, params Guid[] groupIds);
    }
}