namespace MemoryBook.Repository.RelationshipMembership.Extensions
{
    using DataAccess.Entities;
    using Models;

    public static class RelationshipMembershipReadModelExtensions
    {
        public static RelationshipMembershipReadModel ToReadModel(this RelationshipMembership entity)
        {
            return new RelationshipMembershipReadModel
            {
                Id = entity.Id,
                StartDate = entity.StartTime,
                EndDate = entity.EndTime,
                MemberId = entity.MemberId,
                MemberRelationshipTypeId = entity.MemberRelationshipTypeId,
                RelationshipId = entity.RelationshipId
            };
        }
    }
}