namespace MemoryBook.Repository.Detail.Extensions
{
    using System.Linq;
    using DataAccess.Entities;
    using Models;

    public static class DetailReadModelExtensions
    {
        public static DetailReadModel ToReadModel(this Detail entity)
        {
            return new DetailReadModel
            {
                Id = entity.Id,
                CreatorId = entity.CreatorId,
                CustomDetailText = entity.CustomDetailText,
                DetailTypeId = entity.DetailTypeId,
                DetailTypeCode = entity.DetailType.Code,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                Story = entity.Story,
                EditorIds = entity.Permissions?.Where(x => x.CanEdit).Select(x => x.MemberId).ToList(),
                EntityIds = entity.DetailAssociations?.Select(x => x.EntityId).ToList()
            };
        }
    }
}