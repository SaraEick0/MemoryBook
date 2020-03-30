namespace MemoryBook.Business.DetailAssociation.Extensions
{
    using DataAccess.Entities;
    using Models;

    public static class DetailAssociationReadModelExtensions
    {
        public static DetailAssociationReadModel ToReadModel(this DetailAssociation entity)
        {
            return new DetailAssociationReadModel
            {
                Id = entity.Id,
                DetailId = entity.DetailId,
                EntityTypeId = entity.EntityTypeId,
                EntityId = entity.EntityId
            };
        }
    }
}