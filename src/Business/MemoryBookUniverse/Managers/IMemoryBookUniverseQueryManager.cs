namespace MemoryBook.Business.MemoryBookUniverse.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IMemoryBookUniverseQueryManager
    {
        Task<IList<MemoryBookUniverseReadModel>> GetAllMemoryBookUniverses();

        Task<IList<MemoryBookUniverseReadModel>> GetMemoryBookUniverses(params Guid[] universeIds);

        Task<IList<MemoryBookUniverseReadModel>> GetMemoryBookUniverses(params string[] names);
    }
}