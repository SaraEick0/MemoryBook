namespace MemoryBook.Repository.Group.Models
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Interfaces;

    public class GroupReadModel : GroupModelBase, IHasIdProperty
    {
        public Guid Id { get; set; }

        public IList<Guid> MemberIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Note: Not populated by entity framework
        /// </summary>
        public IList<Guid> DetailIds { get; set; } = new List<Guid>();
    }
}