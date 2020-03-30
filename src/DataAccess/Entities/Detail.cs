namespace MemoryBook.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using Interfaces;

    public class Detail : IHasIdProperty
    {
        public Guid Id { get; set; }

        public string CustomDetailText { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public Guid DetailTypeId { get; set; }

        public DetailType DetailType { get; set; }

        public string Story { get; set; }

        public Member Creator { get; set; }

        public Guid CreatorId { get; set; }

        public IList<DetailAssociation> DetailAssociations { get; set; }

        public List<DetailPermission> Permissions { get; set; }
    }
}