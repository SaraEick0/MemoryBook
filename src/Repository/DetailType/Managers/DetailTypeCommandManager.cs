namespace MemoryBook.Repository.DetailType.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Entities;
    using MemoryBook.Common.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DetailTypeCommandManager : IDetailTypeCommandManager
    {
        private readonly MemoryBookDbContext dbContext;

        public DetailTypeCommandManager(MemoryBookDbContext dbContext)
        {
            Contract.RequiresNotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task CreateDetailType(params DetailTypeCreateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            List<DetailType> entities = models.Select(CreateEntity).ToList();

            this.dbContext.AddRange(entities);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateDetailType(params DetailTypeUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            Dictionary<Guid, DetailType> groupDictionary = dbContext.Set<DetailType>()
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<DetailType> entitiesToUpdate = new List<DetailType>();
            foreach (DetailTypeUpdateModel model in models)
            {
                if (model == null || !groupDictionary.TryGetValue(model.Id, out DetailType entity))
                {
                    continue;
                }

                entitiesToUpdate.Add(UpdateEntity(entity, model));
            }

            this.dbContext.AddRange(entitiesToUpdate);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteDetailType(params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            Dictionary<Guid, DetailType> groupDictionary = dbContext.Set<DetailType>()
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<DetailType> entitiesToDelete = new List<DetailType>();
            foreach (Guid id in groupIds)
            {
                if (!groupDictionary.TryGetValue(id, out DetailType entity))
                {
                    continue;
                }

                entitiesToDelete.Add(entity);
            }

            this.dbContext.RemoveRange(entitiesToDelete);

            await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static DetailType CreateEntity(DetailTypeCreateModel model)
        {
            return new DetailType
            {
                Code = model.Code,
                DetailStartText = model.DetailStartText,
                DetailEndText = model.DetailEndText
            };
        }

        private static DetailType UpdateEntity(DetailType entity, DetailTypeUpdateModel model)
        {
            entity.Code = model.Code;
            entity.DetailStartText = model.DetailStartText;
            entity.DetailEndText = model.DetailEndText;

            return entity;
        }
    }
}