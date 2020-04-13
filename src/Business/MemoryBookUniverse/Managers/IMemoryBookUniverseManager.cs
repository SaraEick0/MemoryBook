namespace MemoryBook.Business.MemoryBookUniverse.Managers
{
    using System;
    using System.Threading.Tasks;
    using Repository.MemoryBookUniverse.Models;

    public interface IMemoryBookUniverseManager
    {
        Task<Guid> CreateUniverse(string universeName);

        Task UpdateUniverses(params MemoryBookUniverseUpdateModel[] updateModels);

        Task<MemoryBookUniverseReadModel> GetUniverse(Guid universeId);

        Task<MemoryBookUniverseReadModel> GetUniverse(string universeName);

        Task DeleteUniverse(params Guid[] memoryBookIds);
    }
}