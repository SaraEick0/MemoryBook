namespace MemoryBook.Repository.MemoryBookUniverse.Managers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.MemoryBookUniverse.Managers;
    using Business.MemoryBookUniverse.Models;
    using Common;
    using Common.Extensions;

    public class MemoryBookUniverseManager : IMemoryBookUniverseManager
    {
        private readonly IMemoryBookUniverseQueryManager memoryBookUniverseQueryManager;
        private readonly IMemoryBookUniverseCommandManager memoryBookUniverseCommandManager;

        public MemoryBookUniverseManager(IMemoryBookUniverseQueryManager memoryBookUniverseQueryManager,
            IMemoryBookUniverseCommandManager memoryBookUniverseCommandManager)
        {
            Contract.RequiresNotNull(memoryBookUniverseQueryManager, nameof(memoryBookUniverseQueryManager));
            Contract.RequiresNotNull(memoryBookUniverseCommandManager, nameof(memoryBookUniverseCommandManager));

            this.memoryBookUniverseQueryManager = memoryBookUniverseQueryManager;
            this.memoryBookUniverseCommandManager = memoryBookUniverseCommandManager;
        }

        public async Task<Guid> GetUniverse(string universeName)
        {
            Contract.RequiresNotNullOrWhitespace(universeName, nameof(universeName));

            var universes = await this.memoryBookUniverseQueryManager.GetMemoryBookUniverses(universeName)
                .ConfigureAwait(false);

            if (universes.Any())
            {
                return universes.First().Id;
            }

            return Guid.Empty;
        }

        public async Task<Guid> CreateUniverse(string universeName)
        {
            Contract.RequiresNotNullOrWhitespace(universeName, nameof(universeName));

            var createModel = new MemoryBookUniverseCreateModel
            {
                Name = universeName
            };

            var ids = await this.memoryBookUniverseCommandManager.CreateMemoryBookUniverse(createModel)
                .ConfigureAwait(false);

            return ids.First();
        }

        public async Task DeleteUniverse(Guid memoryBookUniverseId)
        {
            var universes = await this.memoryBookUniverseQueryManager.GetAllMemoryBookUniverses()
                .ConfigureAwait(false);

            if (!universes.Any(x => x.Id == memoryBookUniverseId))
            {
                return;
            }

            await this.memoryBookUniverseCommandManager.DeleteMemoryBookUniverse(memoryBookUniverseId)
                .ConfigureAwait(false);
        }
    }
}