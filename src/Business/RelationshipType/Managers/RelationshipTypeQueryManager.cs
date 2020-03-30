namespace MemoryBook.Business.RelationshipType.Managers
{
    using DataAccess;
    using DataAccess.Entities;
    using Extensions;
    using MemoryBook.Business.RelationshipType.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MemoryBook.Common.Extensions;

    public class RelationshipTypeQueryManager : IRelationshipTypeQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public RelationshipTypeQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<RelationshipTypeReadModel>> GetAllRelationshipTypes()
        {
            return await dbContext.Set<RelationshipType>()
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

            return await dbContext.Set<RelationshipType>()
                .AsNoTracking()
                .Where(x => relationshipTypes.Any(r => r.ToString().Equals(x.Code, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}