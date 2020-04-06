namespace MemoryBook.Business.Detail.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Providers;
    using Repository.Detail.Models;
    using Repository.DetailType;
    using Repository.Member.Models;

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

        public async Task<DetailReadModel> CreateBirthday(Guid memoryBookUniverseId, MemberReadModel creator, Guid memberId, DateTime birthday, string birthplace = null)
        {
            Contract.RequiresNotNull(creator, nameof(creator));

            var detailsForMember = await this.detailProvider.GetDetailsForMembers(memoryBookUniverseId, memberId).ConfigureAwait(false);

            var details = detailsForMember?.FirstOrDefault()?.Details;

            if (details != null && details.Any(x => x.DetailTypeCode.Equals(DetailTypeEnum.LifeSpan.ToString())))
            {
                throw new InvalidOperationException($"Lifespan already existed for member {memberId} for universe {memoryBookUniverseId}");
            }

            string birthplaceText = string.Empty;
            if (!string.IsNullOrWhiteSpace(birthplace))
            {
                birthplaceText = $" in {birthplace}";
            }

            var customDetailText = $"Born {birthday.ToShortDateString()}{birthplaceText}";

            var detail = await this.detailProvider.CreateDetail(memoryBookUniverseId, creator, customDetailText, DetailTypeEnum.LifeSpan, birthday)
                .ConfigureAwait(false);

            if (detail != null)
            {
                await this.detailAssociationProvider.CreateMemberDetailAssociation(detail, new List<Guid> { memberId })
                    .ConfigureAwait(false);
            }

            return detail;
        }

        public async Task<DetailReadModel> CreateEvent(MemberReadModel creator, IList<Guid> memberAttendees, DateTime? startDate, DateTime? endDate, string description)
        {
            Contract.RequiresNotNull(creator, nameof(creator));
            Contract.RequiresNotNullOrEmpty(memberAttendees, nameof(memberAttendees));

            var detail = await this.detailProvider.CreateDetail(creator.MemoryBookUniverseId, creator, description, DetailTypeEnum.Event, startDate, endDate)
                .ConfigureAwait(false);

            await this.detailAssociationProvider.CreateMemberDetailAssociation(detail, memberAttendees)
                .ConfigureAwait(false);

            return detail;
        }
    }
}