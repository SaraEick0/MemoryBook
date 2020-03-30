namespace MemoryBook.Repository.Group.Extensions
{
    using MemoryBook.Business.Group.Models;
    using System;
    using System.Linq;
    using Business.Detail.Models;
    using Business.Member.Models;

    public static class GroupReadModelExtensions
    {
        public static void AddMember(this GroupReadModel group, MemberReadModel member)
        {
            group.Members.Add(member);
        }

        public static void AddDetail(this GroupReadModel group, DetailReadModel detail)
        {
            group.Details.Add(detail);
        }

        public static MemberReadModel GetMember(this GroupReadModel group, string commonName)
        {
            return group.Members.FirstOrDefault(x => x.CommonName.Equals(commonName, StringComparison.OrdinalIgnoreCase));
        }

        public static MemberReadModel GetMember(this GroupReadModel group, Guid memberId)
        {
            return group.Members.FirstOrDefault(x => x.Id == memberId);
        }
    }
}