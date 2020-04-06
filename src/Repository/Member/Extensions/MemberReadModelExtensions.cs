namespace MemoryBook.Repository.Member.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Entities;
    using Models;

    public static class MemberReadModelExtensions
    {
        public static MemberReadModel ToReadModel(this Member entity, IList<Guid> detailIds)
        {
            return new MemberReadModel
            {
                Id = entity.Id,
                CommonName = entity.CommonName,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                MemoryBookUniverseId = entity.MemoryBookUniverseId,
                DetailIds = detailIds,
                GroupIds = entity.GroupMemberships?.Select(x => x.GroupId).ToList(),
                RelationshipIds = entity.RelationshipMemberships?.Select(x => x.RelationshipId).ToList()
            };
        }
    }
}