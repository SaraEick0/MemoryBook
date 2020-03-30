namespace MemoryBook.Repository.Detail.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.Detail.Models;
    using Business.DetailType;
    using Business.Member.Models;
    using Common.Extensions;
    using Member.Extensions;
    using Providers;

    public class MemberDetailManager : IMemberDetailManager
    {
        private readonly IDetailProvider detailProvider;
        private readonly IDetailAssociationProvider detailAssociationProvider;

        public MemberDetailManager(IDetailProvider detailProvider, IDetailAssociationProvider detailAssociationProvider)
        {
            Contract.RequiresNotNull(detailProvider, nameof(detailProvider));
            Contract.RequiresNotNull(detailAssociationProvider, nameof(detailAssociationProvider));

            this.detailProvider = detailProvider;
            this.detailAssociationProvider = detailAssociationProvider;
        }

        public async Task<DetailReadModel> CreateBirthday(MemberReadModel creator, MemberReadModel member, DateTime birthday, string birthplace = null)
        {
            Contract.RequiresNotNull(creator, nameof(creator));
            Contract.RequiresNotNull(member, nameof(member));

            if (member.Details != null && member.Details.Any(x => x.DetailType.Code.Equals(DetailTypeEnum.LifeSpan.ToString())))
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
            Contract.RequiresNotNull(creator, nameof(creator));
            Contract.RequiresNotNullOrEmpty(memberAttendees, nameof(memberAttendees));

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
    }
}