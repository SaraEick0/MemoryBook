namespace MemoryBook.Repository.MemoryBookUniverse.Managers
{
    using System;
    using System.Threading.Tasks;
    using Business.MemoryBookUniverse.Models;

    public interface IMemoryBookUniverseManager
    {
        Task<Guid> CreateUniverse(string universeName);

        Task<MemoryBookUniverseReadModel> GetUniverse(Guid universeId);

        Task<MemoryBookUniverseReadModel> GetUniverse(string universeName);

        Task DeleteUniverse(Guid memoryBookUniverseId);
    }
}