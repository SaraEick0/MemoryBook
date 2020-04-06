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
    using MemoryBook.Repository.Detail.Models;
    using Microsoft.EntityFrameworkCore;

    public class DetailQueryManager : IDetailQueryManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public DetailQueryManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<DetailReadModel>> GetAllDetails(Guid memoryBookUniverseId)
        {
            return await this.GetBaseQuery(memoryBookUniverseId)
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<DetailReadModel>> GetDetails(Guid memoryBookUniverseId, IList<Guid> detailIds)
        {
            if (detailIds == null || detailIds.Count == 0)
            {
                return new List<DetailReadModel>();
            }

            return await this.GetBaseQuery(memoryBookUniverseId)
                .Where(x => detailIds.Contains(x.Id))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<DetailReadModel>> GetDetailsByEntity(Guid memoryBookUniverseId, Guid[] entityIds)
        {
            return await this.GetBaseQuery(memoryBookUniverseId)
                .Where(x => x.DetailAssociations.Any(r => entityIds.Contains(r.EntityId)))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        private IQueryable<Detail> GetBaseQuery(Guid memoryBookUniverseId)
        {
            return databaseContext.Set<Detail>()
                .AsNoTracking()
                .Include(x => x.DetailType)
                .Include(x => x.Permissions)
                .Include(x => x.DetailAssociations)
                .Where(x => x.Creator == null || x.Creator.MemoryBookUniverseId == memoryBookUniverseId);
        }
    }
}