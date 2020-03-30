namespace MemoryBook.DataAccess.Entities
{
    using System;
    using Interfaces;

    public class RelationshipMembership : IHasIdProperty
    {
        public Guid Id { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public Member Member { get; set; }

        public Guid MemberId { get; set; }

        public RelationshipType MemberRelationshipType { get; set; }

        public Guid MemberRelationshipTypeId { get; set; }

        public Relationship Relationship { get; set; }

        public Guid RelationshipId { get; set; }
    }
}