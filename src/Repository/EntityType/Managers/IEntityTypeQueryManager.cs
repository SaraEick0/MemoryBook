namespace MemoryBook.Repository.EntityType.Managers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IEntityTypeQueryManager
    {
        Task<IList<EntityTypeReadModel>> GetAllEntityTypes();

        Task<IList<EntityTypeReadModel>> GetEntityTypes(params EntityTypeEnum[] entityTypes);
    }
}