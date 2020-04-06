namespace MemoryBook.Repository.DetailType.Managers
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

    public class DetailTypeQueryManager : IDetailTypeQueryManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public DetailTypeQueryManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<DetailTypeReadModel>> GetAllDetailTypes()
        {
            return await databaseContext.Set<DetailType>()
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

            return await databaseContext.Set<DetailType>()
                .AsNoTracking()
                .Where(x => detailTypes.Any(r => r.ToString().Equals(x.Code, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}