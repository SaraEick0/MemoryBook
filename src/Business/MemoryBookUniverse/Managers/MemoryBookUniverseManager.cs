namespace MemoryBook.Business.MemoryBookUniverse.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Repository.MemoryBookUniverse.Managers;
    using Repository.MemoryBookUniverse.Models;

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

        public async Task<MemoryBookUniverseReadModel> GetUniverse(Guid universeId)
        {
            IList<MemoryBookUniverseReadModel> universes = await this.memoryBookUniverseQueryManager.GetMemoryBookUniverses(universeId)
                .ConfigureAwait(false);

            if (universes.Any())
            {
                return universes.First();
            }

            return null;
        }

        public async Task<MemoryBookUniverseReadModel> GetUniverse(string universeName)
        {
            Contract.RequiresNotNullOrWhitespace(universeName, nameof(universeName));

            var universes = await this.memoryBookUniverseQueryManager.GetMemoryBookUniverses(universeName)
                .ConfigureAwait(false);

            if (universes.Any())
            {
                return universes.First();
            }

            return null;
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