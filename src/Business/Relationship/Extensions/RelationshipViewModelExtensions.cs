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
    using Repository.RelationshipType.Models;

    public static class RelationshipViewModelExtensions
    {
        public static CombinedRelationshipReadModel ToViewModel(
            this RelationshipReadModel readModel,
            IDictionary<Guid, MemberReadModel> membersDictionary,
            IDictionary<Guid, IList<RelationshipMembershipReadModel>> memberToRelationshipsDictionary,
            IDictionary<Guid, RelationshipTypeReadModel> relationshipTypeDictionary,
            IDictionary<Guid, List<DetailViewModel>> relationshipDetailsDictionary)
        {
            IList<CombinedRelationshipMemberReadModel> relationshipMembers = new List<CombinedRelationshipMemberReadModel>();

            foreach (Guid memberId in readModel.MembershipIds)
            {
                var member = membersDictionary[memberId];
                var memberMembership = memberToRelationshipsDictionary[memberId].FirstOrDefault(x => x.RelationshipId == readModel.Id);
                var memberRelationshipType = relationshipTypeDictionary[memberMembership.MemberRelationshipTypeId];

                relationshipMembers.Add(new CombinedRelationshipMemberReadModel
                {
                    MemberId = memberId,
                    StartDate = memberMembership.StartDate,
                    EndDate = memberMembership.EndDate,
                    Id = memberMembership.Id,
                    MemberRelationshipTypeCode = memberRelationshipType.Code,
                    MemberRelationshipTypeId = memberRelationshipType.Id
                });
            }

            return new CombinedRelationshipReadModel
            {
                Id = readModel.Id,
                StartDate = readModel.StartDate,
                EndDate = readModel.EndDate,
                RelationshipMembers = relationshipMembers,
                Details = relationshipDetailsDictionary.ContainsKey(readModel.Id) ? relationshipDetailsDictionary[readModel.Id] : new List<DetailViewModel>()
            };
        }

        public static DetailViewModel GetDetail(this CombinedRelationshipReadModel member, DetailTypeEnum detailType)
        {
            return member?.Details?.FirstOrDefault(x => x.DetailType == detailType);
        }
    }
}