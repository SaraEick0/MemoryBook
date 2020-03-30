namespace MemoryBook.Business.RelationshipMembership.Extensions
{
    using DataAccess.Entities;
    using Member.Extensions;
    using Models;
    using Relationship.Extensions;
    using RelationshipType.Extensions;

    public static class RelationshipMembershipReadModelExtensions
    {
        public static RelationshipMembershipReadModel ToReadModel(this RelationshipMembership entity)
        {
            return new RelationshipMembershipReadModel
            {
                Id = entity.Id,
                StartDate = entity.StartTime,
                EndDate = entity.EndTime,
                Member = entity.Member.ToShallowReadModel(),
                MemberId = entity.MemberId,
                MemberRelationshipType = entity.MemberRelationshipType.ToReadModel(),
                MemberRelationshipTypeId = entity.MemberRelationshipTypeId,
                RelationshipId = entity.RelationshipId,
                Relationship = entity.Relationship.ToShallowReadModel()
            };
        }
    }
}