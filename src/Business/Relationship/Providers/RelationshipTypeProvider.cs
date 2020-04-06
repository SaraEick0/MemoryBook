namespace MemoryBook.Business.Relationship.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Microsoft.Extensions.Caching.Memory;
    using Repository.RelationshipType;
    using Repository.RelationshipType.Managers;
    using Repository.RelationshipType.Models;

    public class RelationshipTypeProvider : IRelationshipTypeProvider
    {
        private readonly IRelationshipTypeQueryManager relationshipTypeQueryManager;
        private readonly IMemoryCache memoryCache;

        public RelationshipTypeProvider(IRelationshipTypeQueryManager relationshipTypeQueryManager, IMemoryCache memoryCache)
        {
            Contract.RequiresNotNull(relationshipTypeQueryManager, nameof(relationshipTypeQueryManager));
            Contract.RequiresNotNull(memoryCache, nameof(memoryCache));

            this.relationshipTypeQueryManager = relationshipTypeQueryManager;
            this.memoryCache = memoryCache;
        }

        public async Task<IList<RelationshipTypeReadModel>> GetAllRelationshipTypes()
        {
            return await this.GetRelationshipTypes().ConfigureAwait(false);
        }

        public async Task<RelationshipTypeReadModel> GetRelationshipType(RelationshipTypeEnum relationshipType)
        {
            var relationshipTypes = await this.GetRelationshipTypes().ConfigureAwait(false);

            return relationshipTypes.FirstOrDefault(x => x.Code.Equals(relationshipType.ToString()));
        }

        private async Task<IList<RelationshipTypeReadModel>> GetRelationshipTypes()
        {
            string CacheKey = "RelationshipTypes";
            if (this.memoryCache.TryGetValue(CacheKey, out IList<RelationshipTypeReadModel> relationshipTypes))
            {
                return relationshipTypes;
            }

            relationshipTypes = await this.relationshipTypeQueryManager.GetAllRelationshipTypes().ConfigureAwait(false);

            this.memoryCache.Set(CacheKey, relationshipTypes);

            return relationshipTypes;
        }
    }
}