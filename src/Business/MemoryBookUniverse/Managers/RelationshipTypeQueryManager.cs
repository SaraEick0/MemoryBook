namespace MemoryBook.Business.MemoryBookUniverse.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MemoryBookUniverseQueryManager : IMemoryBookUniverseQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public MemoryBookUniverseQueryManager(MemoryBookDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<MemoryBookUniverseReadModel>> GetAllMemoryBookUniverses()
        {
            return await dbContext.Set<MemoryBookUniverse>()
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<MemoryBookUniverseReadModel>> GetMemoryBookUniverses(params string[] names)
        {
            if (names == null || names.Length > 0)
            {
                return new List<MemoryBookUniverseReadModel>();
            }

            return await dbContext.Set<MemoryBookUniverse>()
                .AsNoTracking()
                .Where(x => names.Contains(x.Name, StringComparer.OrdinalIgnoreCase))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}