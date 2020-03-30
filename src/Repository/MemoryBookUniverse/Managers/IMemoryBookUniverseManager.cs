namespace MemoryBook.Repository.MemoryBookUniverse.Managers
{
    using System;
    using System.Threading.Tasks;

    public interface IMemoryBookUniverseManager
    {
        Task<Guid> CreateUniverse(string universeName);

        Task<Guid> GetUniverse(string universeName);

        Task DeleteUniverse(Guid memoryBookUniverseId);
    }
}