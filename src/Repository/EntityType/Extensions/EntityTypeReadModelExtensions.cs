namespace MemoryBook.Repository.EntityType.Extensions
{
    using DataAccess.Entities;
    using Models;

    public static class EntityTypeReadModelExtensions
    {
        public static EntityTypeReadModel ToReadModel(this EntityType entity)
        {
            return new EntityTypeReadModel
            {
                Id = entity.Id,
                Code = entity.Code
            };
        }
    }
}