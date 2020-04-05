using System;

namespace MemoryBook.Business.Detail.Managers
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Member.Extensions;
    using Providers;
    using Repository.Detail.Models;
    using Repository.DetailType;
    using Repository.Member.Models;
    using Repository.Relationship.Models;

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
            RelationshipReadModel relationship,
            DateTime weddingDate,
            string weddingLocation = null)
        {
            Contract.RequiresNotNull(creator, nameof(creator));
            Contract.RequiresNotNull(relationship, nameof(relationship));

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

            await this.detailAssociationProvider.CreateDetailAssociation(detail, relationship)
                .ConfigureAwait(false);

            relationship.AddDetail(detail);

            return detail;
        }
    }
}