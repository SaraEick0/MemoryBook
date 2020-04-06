namespace MemoryBook.Business.Group.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;
    using Detail.Models;
    using Member.Models;
    using Models;
    using Repository.Group.Models;

    public static class GroupViewModelExtensions
    {
        public static GroupViewModel ToViewModel(this GroupReadModel readModel)
        {
            Contract.RequiresNotNull(readModel, nameof(readModel));

            return new GroupViewModel
            {
                GroupDescription = readModel.Description,
                GroupName = readModel.Name
            };
        }

        public static void AddMember(this GroupViewModel group, MemberViewModel member)
        {
            if (group.Members == null)
            {
                group.Members = new List<MemberViewModel>();
            }

            group.Members.Add(member);
        }

        public static void AddDetail(this GroupViewModel group, DetailViewModel detail)
        {
            if (group.Details == null)
            {
                group.Details = new List<DetailViewModel>();
            }

            group.Details.Add(detail);
        }

        public static MemberViewModel GetMember(this GroupViewModel group, string commonName)
        {
            return group.Members.FirstOrDefault(x => x.CommonName.Equals(commonName, StringComparison.OrdinalIgnoreCase));
        }
    }
}