namespace MemoryBook.Repository.Relationship.Models
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Interfaces;

    public class RelationshipReadModel : RelationshipModelBase, IHasIdProperty
    {
        public Guid MemoryBookUniverseId { get; set; }

        public Guid Id { get; set; }

        public IList<Guid> MembershipIds { get; set; }

        /// <summary>
        /// NOTE: Not populated by entity framework!
        /// </summary>
        public IList<Guid> DetailIds { get; set; }
    }
}