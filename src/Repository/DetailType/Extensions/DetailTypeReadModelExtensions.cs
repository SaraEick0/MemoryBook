namespace MemoryBook.Repository.DetailType.Extensions
{
    using DataAccess.Entities;
    using Models;

    public static class DetailTypeReadModelExtensions
    {
        public static DetailTypeReadModel ToReadModel(this DetailType entity)
        {
            return new DetailTypeReadModel
            {
                Id = entity.Id,
                Code = entity.Code,
                DetailStartText = entity.DetailStartText,
                DetailEndText = entity.DetailEndText
            };
        }
    }
}