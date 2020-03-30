namespace MemoryBook.Repository.Detail.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.Detail.Models;
    using Business.DetailType;
    using Business.Member.Models;
    using Member.Extensions;
    using Providers;

    public class DetailManager : IDetailManager
    {
        private readonly IDetailProvider detailProvider;
        private readonly IDetailAssociationProvider detailAssociationProvider;

        public DetailManager(IDetailProvider detailProvider, IDetailAssociationProvider detailAssociationProvider)
        {
            this.detailProvider = detailProvider ?? throw new ArgumentNullException(nameof(detailProvider));
            this.detailAssociationProvider = detailAssociationProvider ?? throw new ArgumentNullException(nameof(detailAssociationProvider));
        }

        public async Task<DetailReadModel> CreateBirthday(MemberReadModel creator, MemberReadModel member, DateTime birthday, string birthplace = null)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (member.Details.Any(x => x.DetailType.Code.Equals(DetailTypeEnum.LifeSpan.ToString())))
            {
                throw new InvalidOperationException($"Lifespan already existed for member {member.CommonName} with id {member.Id} for universe {member.MemoryBookUniverseId}");
            }

            string birthplaceText = string.Empty;
            if (!string.IsNullOrWhiteSpace(birthplace))
            {
                birthplaceText = $" in {birthplace}";
            }

            var customDetailText = $"Born {birthday.ToShortDateString()}{birthplaceText}";

            var detail = await this.detailProvider.CreateDetail(member.MemoryBookUniverseId, creator, customDetailText, DetailTypeEnum.LifeSpan, birthday)
                .ConfigureAwait(false);

            if (detail != null)
            {
                await this.detailAssociationProvider.CreateDetailAssociation(detail, new List<MemberReadModel> { member })
                    .ConfigureAwait(false);

                member.AddDetail(detail);
            }

            return detail;
        }

        public async Task<DetailReadModel> CreateEvent(MemberReadModel creator, IList<MemberReadModel> memberAttendees, DateTime? startDate, DateTime? endDate, string description)
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }
            if (memberAttendees == null)
            {
                throw new ArgumentNullException(nameof(memberAttendees));
            }

            var detail = await this.detailProvider.CreateDetail(creator.MemoryBookUniverseId, creator, description, DetailTypeEnum.Event, startDate, endDate)
                .ConfigureAwait(false);

            await this.detailAssociationProvider.CreateDetailAssociation(detail, memberAttendees)
                .ConfigureAwait(false);

            foreach (var member in memberAttendees)
            {
                member.AddDetail(detail);
            }

            return detail;
        }

        public async Task<DetailReadModel> CreateWedding(MemberReadModel creator, MemberReadModel spouseOne, MemberReadModel spouseTwo, DateTime weddingDate, string weddingLocation = null)
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }
            if (spouseOne == null)
            {
                throw new ArgumentNullException(nameof(spouseOne));
            }
            if (spouseTwo == null)
            {
                throw new ArgumentNullException(nameof(spouseTwo));
            }

            string weddingLocationText = string.Empty;
            if (!string.IsNullOrWhiteSpace(weddingLocation))
            {
                weddingLocationText = $" at {weddingLocation}";
            }

            string customDetailText = $"Married {weddingDate.ToShortDateString()}{weddingLocationText}";

            var detail = await this.detailProvider.CreateDetail(creator.MemoryBookUniverseId, creator, customDetailText, DetailTypeEnum.Wedding, weddingDate)
                .ConfigureAwait(false);

            await this.detailAssociationProvider.CreateDetailAssociation(detail, new List<MemberReadModel> { spouseOne, spouseTwo })
                .ConfigureAwait(false);

            spouseOne.AddDetail(detail);
            spouseTwo.AddDetail(detail);

            return detail;
        }
    }
}