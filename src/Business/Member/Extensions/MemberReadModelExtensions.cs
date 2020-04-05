namespace MemoryBook.Business.Member.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Repository.Detail.Models;
    using Repository.DetailType;
    using Repository.Member.Models;
    using Repository.Relationship.Models;
    using Repository.RelationshipType;

    public static class MemberReadModelExtensions
    {
        public static DetailReadModel GetBirthday(this MemberReadModel member)
        {
            return member?.GetDetail(DetailTypeEnum.LifeSpan);
        }

        /// <summary>
        /// After the detail has been saved to the database, this will just add it to the member model. I don't think we would care about doing this outside of test data.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="detail"></param>
        public static void AddDetail(this MemberReadModel member, DetailReadModel detail)
        {
            if (member.Details == null)
            {
                member.Details = new List<DetailReadModel>();
            }

            member.Details.Add(detail);
        }

        /// <summary>
        /// After the relationship has been saved to the database, this will just add it to the member model. I don't think we would care about doing this outside of test data.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="relationship"></param>
        public static void AddRelationship(this MemberReadModel member, RelationshipReadModel relationship)
        {
            if (member.Relationships == null)
            {
                member.Relationships = new List<RelationshipReadModel>();
            }

            member.Relationships.Add(relationship);
        }

        public static DetailReadModel GetDetail(this MemberReadModel member, DetailTypeEnum detailType)
        {
            return member?.Details?.FirstOrDefault(x => x.DetailType.Code == detailType.ToString());
        }

        public static RelationshipReadModel GetRelationship(this MemberReadModel member, RelationshipTypeEnum otherMemberRelationshipType)
        {
            return member?.Relationships?.FirstOrDefault(x =>
                (x.Memberships.Any(n => n.MemberRelationshipType.Code == otherMemberRelationshipType.ToString())));
        }
    }
}