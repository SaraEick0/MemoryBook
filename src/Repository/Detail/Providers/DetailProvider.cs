using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryBook.Repository.Detail.Providers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Business.Detail.Managers;
    using Business.Detail.Models;
    using Business.DetailType;
    using Business.DetailType.Managers;
    using Business.DetailType.Models;
    using Business.Member.Models;
    using Microsoft.Extensions.Caching.Memory;

    public class DetailProvider : IDetailProvider
    {
        private readonly IDetailTypeQueryManager detailTypeQueryManager;
        private readonly IDetailCommandManager detailCommandManager;
        private readonly IDetailQueryManager detailQueryManager;
        private readonly IMemoryCache memoryCache;

        public DetailProvider(IDetailTypeQueryManager detailTypeQueryManager, IDetailCommandManager detailCommandManager, IDetailQueryManager detailQueryManager, IMemoryCache memoryCache)
        {
            this.detailTypeQueryManager = detailTypeQueryManager ?? throw new ArgumentNullException(nameof(detailTypeQueryManager));
            this.detailCommandManager = detailCommandManager ?? throw new ArgumentNullException(nameof(detailCommandManager));
            this.detailQueryManager = detailQueryManager ?? throw new ArgumentNullException(nameof(detailQueryManager));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<DetailReadModel> CreateDetail(Guid memoryBookUniverseId, MemberReadModel creator, string customDetailText, DetailTypeEnum detailType, DateTime? startDate, DateTime? endDate = null)
        {
            var detailTypeReadModel = await this.GetDetailType(detailType).ConfigureAwait(false);

            DetailCreateModel detailCreateModel = new DetailCreateModel
            {
                StartTime = startDate,
                EndTime = endDate ?? startDate,
                CreatorId = creator.Id,
                CustomDetailText = customDetailText,
                DetailTypeId = detailTypeReadModel.Id
            };

            return await this.CreateDetail(memoryBookUniverseId, detailCreateModel)
                .ConfigureAwait(false);
        }

        private async Task<DetailReadModel> CreateDetail(Guid memoryBookUniverseId, DetailCreateModel detailCreateModel)
        {
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