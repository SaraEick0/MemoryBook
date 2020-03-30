namespace MemoryBook.Repository.Member.Providers
{
    using System.Threading.Tasks;
    using Business.RelationshipType;
    using Business.RelationshipType.Models;

    public interface IRelationshipTypeProvider
    {
        Task<RelationshipTypeReadModel> GetRelationshipType(RelationshipTypeEnum relationshipType);
    }
}