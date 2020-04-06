namespace MemoryBook.Repository.MemoryBookUniverse.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Extensions;
    using MemoryBook.Common.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MemoryBookUniverseQueryManager : IMemoryBookUniverseQueryManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public MemoryBookUniverseQueryManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<MemoryBookUniverseReadModel>> GetAllMemoryBookUniverses()
        {
            return await databaseContext.Set<MemoryBookUniverse>()
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<MemoryBookUniverseReadModel>> GetMemoryBookUniverses(params Guid[] universeIds)
        {
            if (universeIds == null || universeIds.Length == 0)
            {
                return new List<MemoryBookUniverseReadModel>();
            }

            return await databaseContext.Set<MemoryBookUniverse>()
                .AsNoTracking()
                .Where(x => universeIds.Contains(x.Id))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<MemoryBookUniverseReadModel>> GetMemoryBookUniverses(params string[] names)
        {
            if (names == null || names.Length == 0)
            {
                return new List<MemoryBookUniverseReadModel>();
            }

            return await databaseContext.Set<MemoryBookUniverse>()
                .AsNoTracking()
                .Where(x => names.Contains(x.Name))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}