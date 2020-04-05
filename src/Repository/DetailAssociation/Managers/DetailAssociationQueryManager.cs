namespace MemoryBook.Repository.DetailAssociation.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using EntityType;
    using Extensions;
    using MemoryBook.Common.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DetailAssociationQueryManager : IDetailAssociationQueryManager
    {
        private readonly MemoryBookDbContext dbContext;

        public DetailAssociationQueryManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<IList<DetailAssociationReadModel>> GetDetailAssociationByDetailId(params Guid[] detailIds)
        {
            if (detailIds == null || detailIds.Length == 0)
            {
                return new List<DetailAssociationReadModel>();
            }

            return await dbContext.Set<DetailAssociation>()
                .AsNoTracking()
                .Include(x => x.Detail)
                .Include(x => x.EntityType)
                .Where(x => detailIds.Contains(x.Id))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<DetailAssociationReadModel>> GetDetailAssociationByMemberId(params Guid[] memberIds)
        {
            if (memberIds == null || memberIds.Length == 0)
            {
                return new List<DetailAssociationReadModel>();
            }

            return await dbContext.Set<DetailAssociation>()
                .AsNoTracking()
                .Include(x => x.Detail)
                .Include(x => x.EntityType)
                .Where(x => x.EntityType.Code.Equals(EntityTypeEnum.Member.ToString()))
                .Where(x => memberIds.Contains(x.Id))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<DetailAssociationReadModel>> GetDetailAssociationByGroupId(params Guid[] groupIds)
        {
            if (groupIds == null || groupIds.Length == 0)
            {
                return new List<DetailAssociationReadModel>();
            }

            return await dbContext.Set<DetailAssociation>()
                .AsNoTracking()
                .Include(x => x.Detail)
                .Include(x => x.EntityType)
                .Where(x => x.EntityType.Code.Equals(EntityTypeEnum.Group.ToString()))
                .Where(x => groupIds.Contains(x.Id))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }

        public async Task<IList<DetailAssociationReadModel>> GetDetailAssociationByRelationshipId(params Guid[] relationshipIds)
        {
            if (relationshipIds == null || relationshipIds.Length == 0)
            {
                return new List<DetailAssociationReadModel>();
            }

            return await dbContext.Set<DetailAssociation>()
                .AsNoTracking()
                .Include(x => x.Detail)
                .Include(x => x.EntityType)
                .Where(x => x.EntityType.Code.Equals(EntityTypeEnum.Relationship.ToString()))
                .Where(x => relationshipIds.Contains(x.Id))
                .Select(x => x.ToReadModel())
                .ToListAsync();
        }
    }
}