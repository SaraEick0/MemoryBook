namespace MemoryBook.Repository.Member.Extensions
{
    using System.Linq;
    using Business.Detail.Models;
    using Business.DetailType;
    using Business.Member.Models;
    using Business.Relationship.Models;
    using Business.RelationshipType;

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
            member?.Details?.Add(detail);
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