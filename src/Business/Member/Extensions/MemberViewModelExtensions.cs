namespace MemoryBook.Business.Member.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;
    using Detail.Models;
    using Models;
    using Relationship.Models;
    using Repository.DetailType;
    using Repository.Member.Models;
    using Repository.RelationshipType;

    public static class MemberViewModelExtensions
    {
        public static MemberViewModel ToViewModel(this MemberReadModel memberReadModel, IDictionary<Guid, List<DetailViewModel>> memberDetailsDictionary, IList<CombinedRelationshipReadModel> relationships)
        {
            Contract.RequiresNotNull(memberReadModel, nameof(memberReadModel));

            string middleName = string.IsNullOrWhiteSpace(memberReadModel.MiddleName) ? " " : " " + memberReadModel.MiddleName;
            return new MemberViewModel
            {
                CommonName = memberReadModel.CommonName,
                FullName = $"{memberReadModel.FirstName}{middleName} {memberReadModel.LastName}",
                MemoryBookUniverseId = memberReadModel.MemoryBookUniverseId,
                Id = memberReadModel.Id,
                Details = memberDetailsDictionary.ContainsKey(memberReadModel.Id) ? memberDetailsDictionary[memberReadModel.Id] : new List<DetailViewModel>(),
                Relationships = relationships?.Where(x => x.RelationshipMembers.Any(n => n.MemberId == memberReadModel.Id)).ToList()
            };
        }

        public static DetailViewModel GetBirthday(this MemberViewModel member)
        {
            return member?.GetDetail(DetailTypeEnum.LifeSpan);
        }

        public static void AddDetail(this MemberViewModel member, DetailViewModel detail)
        {
            if (member.Details == null)
            {
                member.Details = new List<DetailViewModel>();
            }

            member.Details.Add(detail);
        }

        public static void AddRelationship(this MemberViewModel member, CombinedRelationshipReadModel relationship)
        {
            if (member.Relationships == null)
            {
                member.Relationships = new List<CombinedRelationshipReadModel>();
            }

            member.Relationships.Add(relationship);
        }

        public static DetailViewModel GetDetail(this MemberViewModel member, DetailTypeEnum detailType)
        {
            return member?.Details?.FirstOrDefault(x => x.DetailType == detailType);
        }

        public static CombinedRelationshipReadModel GetRelationship(this MemberViewModel member, RelationshipTypeEnum otherMemberRelationshipType)
        {
            return member?.Relationships?.FirstOrDefault(x =>
                (x.RelationshipMembers.Any(r => r.MemberRelationshipTypeCode == otherMemberRelationshipType.ToString())));
        }
    }
}