namespace MemoryBook.Repository.RelationshipMembership.Models
{
    using System;

    public abstract class RelationshipMembershipModelBase
    {
        public Guid RelationshipId { get; set; }

        public Guid MemberId { get; set; }

        public Guid MemberRelationshipTypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}