namespace MemoryBook.Business.EntityType.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using EntityType;
    using Extensions;
    using MemoryBook.Common.Extensions;
    using Models;

    public class EntityTypeQueryManager : IEntityTypeQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public EntityTypeQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<EntityTypeReadModel>> GetAllEntityTypes()
        {
            return await dbContext.Set<EntityType>()
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<EntityTypeReadModel>> GetEntityTypes(params EntityTypeEnum[] entityTypes)
        {
            if (entityTypes == null || entityTypes.Length > 0)
            {
                return new List<EntityTypeReadModel>();
            }

            return await dbContext.Set<EntityType>()
                .AsNoTracking()
                .Where(x => entityTypes.Any(r => r.ToString().Equals(x.Code, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}