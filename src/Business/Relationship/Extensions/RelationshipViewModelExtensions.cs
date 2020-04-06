namespace MemoryBook.Business.Relationship.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Detail.Models;
    using Models;
    using Repository.DetailType;
    using Repository.Member.Models;
    using Repository.Relationship.Models;
    using Repository.RelationshipMembership.Models;
    using Repository.RelationshipType;
    using Repository.RelationshipType.Models;

    public static class RelationshipViewModelExtensions
    {
        public static RelationshipViewModel ToViewModel(
            this RelationshipReadModel readModel,
            IDictionary<Guid, MemberReadModel> membersDictionary,
            IDictionary<Guid, IList<RelationshipMembershipReadModel>> memberToRelationshipsDictionary,
            IDictionary<Guid, RelationshipTypeReadModel> relationshipTypeDictionary,
            IDictionary<Guid, List<DetailViewModel>> relationshipDetailsDictionary)
        {
            Guid firstMemberId = readModel.MembershipIds.First();
            var firstMember = membersDictionary[firstMemberId];
            var firstMemberMembership = memberToRelationshipsDictionary[firstMemberId].FirstOrDefault(x => x.RelationshipId == readModel.Id);
            Enum.TryParse<RelationshipTypeEnum>(relationshipTypeDictionary[firstMemberMembership.MemberRelationshipTypeId].Code, out var firstMemberRelationshipType);

            Guid secondMemberId = readModel.MembershipIds.Last();
            var secondMember = membersDictionary[secondMemberId];
            var secondMemberMembership = memberToRelationshipsDictionary[secondMemberId].FirstOrDefault(x => x.RelationshipId == readModel.Id);
            Enum.TryParse<RelationshipTypeEnum>(relationshipTypeDictionary[secondMemberMembership.MemberRelationshipTypeId].Code, out var secondMemberRelationshipType);

            return new RelationshipViewModel
            {
                StartDate = readModel.StartDate,
                EndDate = readModel.EndDate,
                FirstMemberId = firstMemberId,
                FirstMemberName = firstMember.CommonName,
                FirstMemberRelationshipType = firstMemberRelationshipType,
                SecondMemberId = secondMemberId,
                SecondMemberName = secondMember.CommonName,
                SecondMemberRelationshipType = secondMemberRelationshipType,
                Details = relationshipDetailsDictionary.ContainsKey(readModel.Id) ? relationshipDetailsDictionary[readModel.Id] : new List<DetailViewModel>()
            };
        }

        public static DetailViewModel GetDetail(this RelationshipViewModel member, DetailTypeEnum detailType)
        {
            return member?.Details?.FirstOrDefault(x => x.DetailType == detailType);
        }
    }
}