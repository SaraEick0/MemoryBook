namespace MemoryBook.Business.Group.Models
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Interfaces;
    using Detail.Models;
    using Member.Models;

    public class GroupReadModel : GroupModelBase, IHasIdProperty
    {
        public Guid Id { get; set; }

        public List<MemberReadModel> Members { get; set; } = new List<MemberReadModel>();

        /// <summary>
        /// Note: Not populated by entity framework
        /// </summary>
        public List<DetailReadModel> Details { get; set; } = new List<DetailReadModel>();
    }
}