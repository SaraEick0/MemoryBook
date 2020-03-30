namespace MemoryBook.Repository.Member.Models
{
    using Business.Member.Models;
    using Business.RelationshipType.Models;

    public class RelationshipMemberModel
    {
        public MemberReadModel Member { get; set; }

        public RelationshipTypeReadModel RelationshipType { get; set; }
    }
}