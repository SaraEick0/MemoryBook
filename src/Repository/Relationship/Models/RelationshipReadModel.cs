namespace MemoryBook.Repository.Relationship.Models
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Interfaces;
    using Detail.Models;
    using RelationshipMembership.Models;

    public class RelationshipReadModel : RelationshipModelBase, IHasIdProperty
    {
        public Guid MemoryBookUniverseId { get; set; }

        public Guid Id { get; set; }

        public IList<RelationshipMembershipReadModel> Memberships { get; set; }

        /// <summary>
        /// NOTE: Not populated by entity framework!
        /// </summary>
        public IList<DetailReadModel> Details { get; set; }
    }
}