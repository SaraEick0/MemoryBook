namespace MemoryBook.Repository.Detail.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IDetailQueryManager
    {
        Task<IList<DetailReadModel>> GetAllDetails(Guid memoryBookUniverseId);

        Task<IList<DetailReadModel>> GetDetails(Guid memoryBookUniverseId, IList<Guid> detailIds);
    }
}