namespace MemoryBook.Repository.Group.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Entities;
    using Models;

    public static class GroupReadModelExtensions
    {
        public static GroupReadModel ToReadModel(this Group group, IList<Guid> detailIds)
        {
            return new GroupReadModel
            {
                Id =  group.Id,
                Name = group.Name,
                Code = group.Code,
                Description = group.Description,
                MemberIds = group.GroupMemberships?.Select(x => x.MemberId).ToList(),
                DetailIds = detailIds
            };
        }
    }
}