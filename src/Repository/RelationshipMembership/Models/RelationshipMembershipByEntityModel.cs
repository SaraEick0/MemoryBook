namespace MemoryBook.Repository.RelationshipMembership.Models
{
    using System;
    using System.Collections.Generic;

    public class RelationshipMembershipByEntityModel
    {
        public Guid EntityId { get; set; }

        public IList<RelationshipMembershipReadModel> RelationshipMemberships { get; set; }
    }
}