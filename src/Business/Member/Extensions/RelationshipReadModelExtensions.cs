namespace MemoryBook.Business.Member.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Repository.Detail.Models;
    using Repository.DetailType;
    using Repository.Member.Models;
    using Repository.Relationship.Models;

    public static class RelationshipReadModelExtensions
    {
        public static void AddDetail(this RelationshipReadModel relationship, DetailReadModel detail)
        {
            if (relationship.Details == null)
            {
                relationship.Details = new List<DetailReadModel>();
            }

            relationship.Details.Add(detail);
        }

        public static DetailReadModel GetDetail(this RelationshipReadModel relationship, DetailTypeEnum detailType)
        {
            return relationship?.Details?.FirstOrDefault(x => x.DetailTypeText == detailType.ToString());
        }

        public static MemberReadModel GetMember(this RelationshipReadModel relationship, string commonName)
        {
            return relationship.Memberships
                .FirstOrDefault(x => x.Member.CommonName.Equals(commonName, StringComparison.OrdinalIgnoreCase)).Member;
        }

        public static MemberReadModel GetMember(this RelationshipReadModel relationship, Guid memberId)
        {
            return relationship.Memberships
                .FirstOrDefault(x => x.MemberId.Equals(memberId)).Member;
        }
    }
}