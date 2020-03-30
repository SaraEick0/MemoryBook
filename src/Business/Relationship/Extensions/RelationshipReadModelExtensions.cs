namespace MemoryBook.Business.Relationship.Extensions
{
    using System.Linq;
    using DataAccess.Entities;
    using Detail.Extensions;
    using Models;
    using RelationshipMembership.Extensions;

    public static class RelationshipReadModelExtensions
    {
        public static RelationshipReadModel ToReadModel(this Relationship entity)
        {
            return new RelationshipReadModel
            {
                Id = entity.Id,
                Memberships = entity.RelationshipMemberships?.Select(x=> x.ToReadModel()).ToList(),
                StartDate = entity.StartTime,
                EndDate = entity.EndTime,
                Details = entity.DetailAssociations?.Select(x => x.Detail.ToReadModel()).ToList()
            };
        }
    }
}