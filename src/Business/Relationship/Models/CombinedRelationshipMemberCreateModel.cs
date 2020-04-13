namespace MemoryBook.Business.Relationship.Models
{
    using System;

    public class CombinedRelationshipMemberCreateModel
    {
        public Guid MemberId { get; set; }

        public Guid MemberRelationshipTypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}