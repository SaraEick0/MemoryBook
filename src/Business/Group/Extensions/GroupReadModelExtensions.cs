namespace MemoryBook.Business.Group.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Repository.Detail.Models;
    using Repository.Group.Models;
    using Repository.Member.Models;

    public static class GroupReadModelExtensions
    {
        public static void AddMember(this GroupReadModel group, MemberReadModel member)
        {
            if (group.Members == null)
            {
                group.Members = new List<MemberReadModel>();
            }

            group.Members.Add(member);
        }

        public static void AddDetail(this GroupReadModel group, DetailReadModel detail)
        {
            if (group.Details == null)
            {
                group.Details = new List<DetailReadModel>();
            }

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