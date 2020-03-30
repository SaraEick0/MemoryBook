namespace MemoryBook.Business.DetailAssociation.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DetailAssociationCommandManager : IDetailAssociationCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public DetailAssociationCommandManager(MemoryBookDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IList<Guid>> CreateDetailAssociation(params DetailAssociationCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return new List<Guid>();
            }

            IEnumerable<DetailAssociation> entities = models.Select(model => CreateEntity(model));

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entities.Select(x => x.Id).ToList();
        }

        public async Task DeleteDetailAssociations(params Guid[] detailAssociationIds)
        {
            if (detailAssociationIds == null || !detailAssociationIds.Any())
            {
                return;
            }

            var detailAssociations = await dbContext.Set<DetailAssociation>()
                .Where(x => detailAssociationIds.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            this.dbContext.RemoveRange(detailAssociations);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static DetailAssociation CreateEntity(DetailAssociationCreateModel model)
        {
            return new DetailAssociation
            {
                EntityTypeId = model.EntityTypeId,
                EntityId = model.EntityId,
                DetailId = model.DetailId
            };
        }
    }
}