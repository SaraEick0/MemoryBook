namespace MemoryBook.Repository.Detail.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IDetailCommandManager
    {
        Task<IList<Guid>> CreateDetails(Guid memoryBookUniverseId, params DetailCreateModel[] models);

        Task UpdateDetails(Guid memoryBookUniverseId, params DetailUpdateModel[] models);

        Task DeleteDetails(Guid memoryBookUniverseId, params Guid[] groupIds);
    }
}