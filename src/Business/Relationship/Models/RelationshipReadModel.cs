namespace MemoryBook.Business.Relationship.Models
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Interfaces;
    using Detail.Models;
    using RelationshipMembership.Models;

    public class RelationshipReadModel : RelationshipModelBase, IHasIdProperty
    {
        public Guid Id { get; set; }

        public IList<RelationshipMembershipReadModel> Memberships { get; set; }

        public IList<DetailReadModel> Details { get; set; }
    }
}