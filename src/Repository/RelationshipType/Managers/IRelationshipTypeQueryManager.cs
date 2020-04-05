namespace MemoryBook.Repository.RelationshipType.Managers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRelationshipTypeQueryManager
    {
        Task<IList<RelationshipTypeReadModel>> GetAllRelationshipTypes();

        Task<IList<RelationshipTypeReadModel>> GetRelationshipTypes(params RelationshipTypeEnum[] relationshipTypes);
    }
}