namespace MemoryBook.Repository.MemoryBookUniverse.Managers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.MemoryBookUniverse.Managers;
    using Business.MemoryBookUniverse.Models;

    public class MemoryBookUniverseManager : IMemoryBookUniverseManager
    {
        private readonly IMemoryBookUniverseQueryManager memoryBookUniverseQueryManager;
        private readonly IMemoryBookUniverseCommandManager memoryBookUniverseCommandManager;

        public MemoryBookUniverseManager(IMemoryBookUniverseQueryManager memoryBookUniverseQueryManager,
            IMemoryBookUniverseCommandManager memoryBookUniverseCommandManager)
        {
            this.memoryBookUniverseQueryManager = memoryBookUniverseQueryManager ?? throw new ArgumentNullException(nameof(memoryBookUniverseQueryManager));
            this.memoryBookUniverseCommandManager = memoryBookUniverseCommandManager ?? throw new ArgumentNullException(nameof(memoryBookUniverseCommandManager));
        }

        public async Task<Guid> GetOrCreateUniverse(string universeName)
        {
            var universes = await this.memoryBookUniverseQueryManager.GetMemoryBookUniverses(universeName)
                .ConfigureAwait(false);

            if (universes.Any())
            {
                return universes.First().Id;
            }

            var createModel = new MemoryBookUniverseCreateModel
            {
                Name = universeName
            };

            var ids = await this.memoryBookUniverseCommandManager.CreateMemoryBookUniverse(createModel)
                .ConfigureAwait(false);

            return ids.First();
        }
    }
}