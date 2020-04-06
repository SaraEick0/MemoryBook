namespace MemoryBook.Repository.RelationshipType.Managers
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

    public class RelationshipTypeQueryManager : IRelationshipTypeQueryManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public RelationshipTypeQueryManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<IList<RelationshipTypeReadModel>> GetAllRelationshipTypes()
        {
            return await databaseContext.Set<RelationshipType>()
                .AsNoTracking()
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<RelationshipTypeReadModel>> GetRelationshipTypes(params RelationshipTypeEnum[] relationshipTypes)
        {
            if (relationshipTypes == null || relationshipTypes.Length > 0)
            {
                return new List<RelationshipTypeReadModel>();
            }

            return await databaseContext.Set<RelationshipType>()
                .AsNoTracking()
                .Where(x => relationshipTypes.Any(r => r.ToString().Equals(x.Code, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}