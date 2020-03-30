namespace MemoryBook.Business.Detail.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities;
    using Extensions;
    using MemoryBook.Business.Detail.Models;
    using MemoryBook.DataAccess;
    using Microsoft.EntityFrameworkCore;

    public class DetailQueryManager : IDetailQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public DetailQueryManager(MemoryBookDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<DetailReadModel>> GetAllDetails(Guid memoryBookUniverseId)
        {
            return await dbContext.Set<Detail>()
                .AsNoTracking()
                .Where(x => x.Creator == null || x.Creator.MemoryBookUniverseId == memoryBookUniverseId)
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<DetailReadModel>> GetDetails(Guid memoryBookUniverseId, IList<Guid> detailIds)
        {
            if (detailIds == null || detailIds.Count == 0)
            {
                return new List<DetailReadModel>();
            }

            return await dbContext.Set<Detail>()
                .AsNoTracking()
                .Where(x => x.Creator == null || x.Creator.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => detailIds.Contains(x.Id))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}