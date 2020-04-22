namespace MemoryBook.Repository.EntityType.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Extensions;
    using MemoryBook.Common.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class EntityTypeQueryManager : IEntityTypeQueryManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public EntityTypeQueryManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<EntityTypeReadModel>> GetAllEntityTypes()
        {
            return await databaseContext.Set<EntityType>()
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

            return await databaseContext.Set<EntityType>()
                .AsNoTracking()
                .Where(x => entityTypes.Any(r => r.ToString().Equals(x.Code)))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}