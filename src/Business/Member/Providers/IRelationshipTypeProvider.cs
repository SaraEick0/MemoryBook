namespace MemoryBook.Business.Member.Providers
{
    using System.Threading.Tasks;
    using Repository.RelationshipType;
    using Repository.RelationshipType.Models;

    public interface IRelationshipTypeProvider
    {
        Task<RelationshipTypeReadModel> GetRelationshipType(RelationshipTypeEnum relationshipType);
    }
}