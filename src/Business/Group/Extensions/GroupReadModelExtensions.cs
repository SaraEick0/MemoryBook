namespace MemoryBook.Business.Group.Extensions
{
    using System.Linq;
    using DataAccess.Entities;
    using Detail.Extensions;
    using Member.Extensions;
    using MemoryBook.Business.Group.Models;

    public static class GroupReadModelExtensions
    {
        public static GroupReadModel ToReadModel(this Group group)
        {
            return new GroupReadModel
            {
                Id =  group.Id,
                Name = group.Name,
                Code = group.Code,
                Description = group.Description,
                Members = group.GroupMemberships?.Select(x => x.Member.ToShallowReadModel()).ToList()
            };
        }
    }
}