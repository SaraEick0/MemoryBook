namespace MemoryBook.Business.Relationship.Extensions
{
    using System.Linq;
    using DataAccess.Entities;
    using Models;
    using RelationshipMembership.Extensions;

    public static class RelationshipReadModelExtensions
    {
        public static RelationshipReadModel ToReadModel(this Relationship entity)
        {
            var model = entity.ToShallowReadModel();

            model.Memberships = entity.RelationshipMemberships?.Select(x => x.ToReadModel()).ToList();

            return model;
        }

        public static RelationshipReadModel ToShallowReadModel(this Relationship entity)
        {
            return new RelationshipReadModel
            {
                Id = entity.Id,
                StartDate = entity.StartTime,
                EndDate = entity.EndTime,
                MemoryBookUniverseId = entity.MemoryBookUniverseId
            };
        }
    }
}