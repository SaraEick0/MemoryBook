namespace MemoryBook.Business.Relationship.Models
{
    using System;

    public class CombinedRelationshipMemberReadModel : CombinedRelationshipMemberCreateModel
    {
        public Guid Id { get; set; }

        public string MemberRelationshipTypeCode { get; set; }
    }
}