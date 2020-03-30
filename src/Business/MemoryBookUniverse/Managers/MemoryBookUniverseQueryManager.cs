namespace MemoryBook.Business.MemoryBookUniverse.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Extensions;
    using MemoryBook.Common;
    using MemoryBook.Common.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MemoryBookUniverseQueryManager : IMemoryBookUniverseQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public MemoryBookUniverseQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
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
            if (names == null || names.Length == 0)
            {
                return new List<MemoryBookUniverseReadModel>();
            }

            return await dbContext.Set<MemoryBookUniverse>()
                .AsNoTracking()
                .Where(x => names.Contains(x.Name))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}