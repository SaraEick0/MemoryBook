namespace MemoryBook.Business.Relationship.Models
{
    using Repository.Member.Models;
    using Repository.RelationshipType.Models;

    public class RelationshipMemberModel
    {
        public MemberReadModel Member { get; set; }

        public RelationshipTypeReadModel RelationshipType { get; set; }
    }
}