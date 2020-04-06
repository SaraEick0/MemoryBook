using System;

namespace MemoryBook.Business.Detail.Managers
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Providers;
    using Repository.Detail.Models;
    using Repository.DetailType;
    using Repository.Member.Models;

    public class RelationshipDetailManager : IRelationshipDetailManager
    {
        private readonly IDetailProvider detailProvider;
        private readonly IDetailAssociationProvider detailAssociationProvider;

        public RelationshipDetailManager(IDetailProvider detailProvider, IDetailAssociationProvider detailAssociationProvider)
        {
            Contract.RequiresNotNull(detailProvider, nameof(detailProvider));
            Contract.RequiresNotNull(detailAssociationProvider, nameof(detailAssociationProvider));

            this.detailProvider = detailProvider;
            this.detailAssociationProvider = detailAssociationProvider;
        }

        public async Task<DetailReadModel> CreateWedding(
            MemberReadModel creator,
            Guid relationshipId,
            DateTime weddingDate,
            string weddingLocation = null)
        {
            Contract.RequiresNotNull(creator, nameof(creator));

            string weddingLocationText = string.Empty;
            if (!string.IsNullOrWhiteSpace(weddingLocation))
            {
                weddingLocationText = $" at {weddingLocation}";
            }

            string customDetailText = $"Married {weddingDate.ToShortDateString()}{weddingLocationText}";

            var detail = await this.detailProvider.CreateDetail(
                    creator.MemoryBookUniverseId,
                    creator, customDetailText,
                    DetailTypeEnum.Wedding,
                    weddingDate)
                .ConfigureAwait(false);

            await this.detailAssociationProvider.CreateRelationshipDetailAssociation(detail, relationshipId)
                .ConfigureAwait(false);

            return detail;
        }
    }
}