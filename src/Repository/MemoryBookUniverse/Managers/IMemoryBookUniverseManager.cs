namespace MemoryBook.Repository.MemoryBookUniverse.Managers
{
    using System;
    using System.Threading.Tasks;

    public interface IMemoryBookUniverseManager
    {
        Task<Guid> GetOrCreateUniverse(string universeName);
    }
}