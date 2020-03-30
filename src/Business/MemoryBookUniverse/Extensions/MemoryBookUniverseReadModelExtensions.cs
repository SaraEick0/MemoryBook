namespace MemoryBook.Business.MemoryBookUniverse.Extensions
{
    using DataAccess.Entities;
    using Models;

    public static class MemoryBookUniverseReadModelExtensions
    {
        public static MemoryBookUniverseReadModel ToReadModel(this MemoryBookUniverse entity)
        {
            return new MemoryBookUniverseReadModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedDate = entity.CreatedDate
            };
        }
    }
}