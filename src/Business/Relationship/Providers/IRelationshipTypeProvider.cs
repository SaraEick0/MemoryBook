namespace MemoryBook.Business.Relationship.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repository.RelationshipType;
    using Repository.RelationshipType.Models;

    public interface IRelationshipTypeProvider
    {
        Task<IList<RelationshipTypeReadModel>> GetAllRelationshipTypes();

        Task<RelationshipTypeReadModel> GetRelationshipType(RelationshipTypeEnum relationshipType);
    }
}