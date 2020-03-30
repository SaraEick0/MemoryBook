namespace MemoryBook.Business.RelationshipType.Managers
{
    using System;
    using System.Threading.Tasks;
    using Models;

    public interface IRelationshipTypeCommandManager
    {
        Task CreateRelationshipType(params RelationshipTypeCreateModel[] models);

        Task UpdateRelationshipType(params RelationshipTypeUpdateModel[] models);

        Task DeleteRelationshipType(params Guid[] groupIds);
    }
}