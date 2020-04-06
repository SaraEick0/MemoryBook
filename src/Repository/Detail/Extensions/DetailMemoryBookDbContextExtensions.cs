namespace MemoryBook.Repository.Detail.Extensions
{
    using MemoryBook.DataAccess;
    using MemoryBook.Repository.EntityType;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class DetailMemoryBookDbContextExtensions
    {
        public static async Task<ILookup<Guid, Guid>> GetDetailLookup(this MemoryBookDbContext databaseContext, Guid memoryBookUniverseId, EntityTypeEnum entityType)
        {
            List<DataAccess.Entities.DetailAssociation> groupDetailAssociations = await databaseContext.Set<DataAccess.Entities.DetailAssociation>()
                .AsNoTracking()
                .Where(x => x.EntityType.Code == entityType.ToString())
                .Where(x => x.Detail.Creator.MemoryBookUniverseId == memoryBookUniverseId)
                .ToListAsync();

            return groupDetailAssociations.ToLookup(x => x.EntityId, x => x.DetailId);
        }
    }
}