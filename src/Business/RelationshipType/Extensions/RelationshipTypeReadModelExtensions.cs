namespace MemoryBook.Business.RelationshipType.Extensions
{
    using DataAccess.Entities;
    using Models;

    public static class RelationshipTypeReadModelExtensions
    {
        public static RelationshipTypeReadModel ToReadModel(this RelationshipType entity)
        {
            return new RelationshipTypeReadModel
            {
                Id = entity.Id,
                Code = entity.Code
            };
        }
    }
}