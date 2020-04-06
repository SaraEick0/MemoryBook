namespace MemoryBook.Repository.Relationship.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Entities;
    using Models;

    public static class RelationshipReadModelExtensions
    {
        public static RelationshipReadModel ToReadModel(this Relationship entity, IList<Guid> detailIds)
        {
            return new RelationshipReadModel
            {
                Id = entity.Id,
                StartDate = entity.StartTime,
                EndDate = entity.EndTime,
                MemoryBookUniverseId = entity.MemoryBookUniverseId,
                MembershipIds = entity.RelationshipMemberships?.Select(x => x.MemberId).ToList(),
                DetailIds = detailIds
            };
        }
    }
}