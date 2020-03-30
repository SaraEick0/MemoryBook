namespace MemoryBook.Business.Member.Extensions
{
    using System.Linq;
    using DataAccess.Entities;
    using Detail.Extensions;
    using Group.Extensions;
    using Models;
    using Relationship.Extensions;

    public static class MemberReadModelExtensions
    {
        public static MemberReadModel ToReadModel(this Member entity)
        {
            var model = entity.ToShallowReadModel();

            model.Details = entity.CreatedDetails?.Select(x => x.ToShallowReadModel()).ToList();
            model.Groups = entity.GroupMemberships?.Select(x => x.Group.ToReadModel()).ToList();
            model.Relationships = entity.RelationshipMemberships?.Select(x => x.Relationship.ToShallowReadModel()).ToList();

            return model;
        }

        public static MemberReadModel ToShallowReadModel(this Member entity)
        {
            return new MemberReadModel
            {
                Id = entity.Id,
                CommonName = entity.CommonName,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                MemoryBookUniverseId = entity.MemoryBookUniverseId
            };
        }
    }
}