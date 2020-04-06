namespace MemoryBook.Repository.RelationshipMembership.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RelationshipMembershipByMemberModel
    {
        public Guid MemberId { get; set; }

        public IList<RelationshipMembershipReadModel> RelationshipMemberships { get; set; }
    }
}