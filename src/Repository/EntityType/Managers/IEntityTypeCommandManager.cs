namespace MemoryBook.Repository.EntityType.Managers
{
    using System;
    using System.Threading.Tasks;
    using Models;

    public interface IEntityTypeCommandManager
    {
        Task CreateEntityType(params EntityTypeCreateModel[] models);

        Task UpdateEntityType(params EntityTypeUpdateModel[] models);

        Task DeleteEntityType(params Guid[] groupIds);
    }
}