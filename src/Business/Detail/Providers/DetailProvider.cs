﻿namespace MemoryBook.Business.Detail.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using MemoryBook.Business.Detail.Models;
    using MemoryBook.Repository.Detail.Models;
    using Microsoft.Extensions.Caching.Memory;
    using Repository.Detail.Managers;
    using Repository.DetailType;
    using Repository.DetailType.Managers;
    using Repository.DetailType.Models;
    using Repository.Member.Models;

    public class DetailProvider : IDetailProvider
    {
        private readonly IDetailTypeQueryManager detailTypeQueryManager;
        private readonly IDetailCommandManager detailCommandManager;
        private readonly IDetailQueryManager detailQueryManager;
        private readonly IMemoryCache memoryCache;

        public DetailProvider(IDetailTypeQueryManager detailTypeQueryManager, IDetailCommandManager detailCommandManager, IDetailQueryManager detailQueryManager, IMemoryCache memoryCache)
        {
            Contract.RequiresNotNull(detailTypeQueryManager, nameof(detailTypeQueryManager));
            Contract.RequiresNotNull(detailCommandManager, nameof(detailCommandManager));
            Contract.RequiresNotNull(detailQueryManager, nameof(detailQueryManager));
            Contract.RequiresNotNull(memoryCache, nameof(memoryCache));

            this.detailTypeQueryManager = detailTypeQueryManager;
            this.detailCommandManager = detailCommandManager;
            this.detailQueryManager = detailQueryManager;
            this.memoryCache = memoryCache;
        }

        public async Task<DetailReadModel> CreateDetail(Guid memoryBookUniverseId, MemberReadModel creator, string customDetailText, DetailTypeEnum detailType, DateTime? startDate, DateTime? endDate = null)
        {
            Contract.RequiresNotNull(creator, nameof(creator));

            var detailTypeReadModel = await this.GetDetailType(detailType).ConfigureAwait(false);

            DetailCreateModel detailCreateModel = new DetailCreateModel
            {
                StartTime = startDate,
                EndTime = endDate,
                CreatorId = creator.Id,
                CustomDetailText = customDetailText,
                DetailTypeId = detailTypeReadModel.Id
            };

            return await this.CreateDetail(memoryBookUniverseId, detailCreateModel)
                .ConfigureAwait(false);
        }

        public async Task<IList<DetailsByEntityModel>> GetDetailsForGroups(Guid memoryBookUniverseId, params Guid[] groupIds)
        {
            return await this.GetDetailsForEntities(memoryBookUniverseId, groupIds)
                .ConfigureAwait(false);
        }

        public async Task<IList<DetailsByEntityModel>> GetDetailsForMembers(Guid memoryBookUniverseId, params Guid[] memberIds)
        {
            return await this.GetDetailsForEntities(memoryBookUniverseId, memberIds)
                .ConfigureAwait(false);
        }

        public async Task<IList<DetailsByEntityModel>> GetDetailsForRelationships(Guid memoryBookUniverseId, params Guid[] relationshipIds)
        {
            return await this.GetDetailsForEntities(memoryBookUniverseId, relationshipIds)
                .ConfigureAwait(false);
        }

        private async Task<IList<DetailsByEntityModel>> GetDetailsForEntities(Guid memoryBookUniverseId, params Guid[] entityIds)
        {
            IList<DetailReadModel> details = await this.detailQueryManager.GetDetailsByEntity(memoryBookUniverseId, entityIds).ConfigureAwait(false);

            return details?.SelectMany(x => x.EntityIds.Select(entityId => new { EntityId = entityId, Detail = x }))
                .GroupBy(x => x.EntityId)
                .Select(x => new DetailsByEntityModel
                {
                    EntityId = x.Key,
                    Details = x.Select(p => p.Detail).ToList()
                }).ToList();
        }

        private async Task<DetailReadModel> CreateDetail(Guid memoryBookUniverseId, DetailCreateModel detailCreateModel)
        {
            Contract.RequiresNotNull(detailCreateModel, nameof(detailCreateModel));

            var id = await this.detailCommandManager.CreateDetails(memoryBookUniverseId, detailCreateModel)
                .ConfigureAwait(false);

            if (id == null || id.Count == 0)
            {
                return null;
            }

            var detailReadModels = await this.detailQueryManager.GetDetails(memoryBookUniverseId, id)
                .ConfigureAwait(false);

            return detailReadModels.FirstOrDefault();
        }

        private async Task<DetailTypeReadModel> GetDetailType(DetailTypeEnum detailType)
        {
            var types = await this.GetDetailTypes().ConfigureAwait(false);

            return types.First(x => x.Code.Equals(detailType.ToString()));
        }

        private async Task<IList<DetailTypeReadModel>> GetDetailTypes()
        {
            const string CacheKey = "DetailTypes";

            if (this.memoryCache.TryGetValue(CacheKey, out IList<DetailTypeReadModel> entityTypes))
            {
                return entityTypes;
            }

            entityTypes = await this.detailTypeQueryManager.GetAllDetailTypes().ConfigureAwait(false);

            this.memoryCache.Set(CacheKey, entityTypes);

            return entityTypes;
        }
    }
}