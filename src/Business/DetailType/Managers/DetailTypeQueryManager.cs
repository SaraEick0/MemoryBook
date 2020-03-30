namespace MemoryBook.Business.DetailType.Managers
{
    using DataAccess;
    using DataAccess.Entities;
    using Extensions;
    using MemoryBook.Business.DetailType.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MemoryBook.Common;
    using MemoryBook.Common.Extensions;

    public class DetailTypeQueryManager : IDetailTypeQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public DetailTypeQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<DetailTypeReadModel>> GetAllDetailTypes()
        {
            return await dbContext.Set<DetailType>()
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<DetailTypeReadModel>> GetDetailTypes(params DetailTypeEnum[] detailTypes)
        {
            if (detailTypes == null || detailTypes.Length > 0)
            {
                return new List<DetailTypeReadModel>();
            }

            return await dbContext.Set<DetailType>()
                .AsNoTracking()
                .Where(x => detailTypes.Any(r => r.ToString().Equals(x.Code, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}