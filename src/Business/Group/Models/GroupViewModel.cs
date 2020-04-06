namespace MemoryBook.Business.Group.Models
{
    using System.Collections.Generic;
    using Detail.Models;
    using Member.Models;

    public class GroupViewModel
    {
        public string GroupName { get; set; }

        public string GroupDescription { get; set; }

        public IList<MemberViewModel> Members { get; set; }

        public IList<DetailViewModel> Details { get; set; }
    }
}