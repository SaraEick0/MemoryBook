namespace MemoryBook.Business.RelationshipMembership.Models
{
    using System;
    using Member.Models;
    using Relationship.Models;
    using RelationshipType.Models;

    public class RelationshipMembershipReadModel : RelationshipMembershipModelBase
    {
        public Guid Id { get; set; }

        public MemberReadModel Member { get; set; }

        public RelationshipTypeReadModel MemberRelationshipType { get; set; }

        public RelationshipReadModel Relationship { get; set; }
    }
}