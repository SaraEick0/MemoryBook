namespace MemoryBook.Repository.Detail.Managers
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

    public class DetailQueryManager : IDetailQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public DetailQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<DetailReadModel>> GetAllDetails(Guid memoryBookUniverseId)
        {
            return await dbContext.Set<Detail>()
                .AsNoTracking()
                .Include(x => x.DetailType)
                .Include(x => x.Creator)
                .Include(x => x.Permissions)
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
                .Include(x => x.DetailType)
                .Include(x => x.Creator)
                .Include(x => x.Permissions)
                .Where(x => x.Creator == null || x.Creator.MemoryBookUniverseId == memoryBookUniverseId)
                .Where(x => detailIds.Contains(x.Id))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}