namespace MemoryBook.Business.Detail.Extensions
{
    using System;
    using System.Linq;
    using DataAccess.Entities;
    using DetailType.Extensions;
    using EntityType;
    using Member.Extensions;
    using Models;

    public static class DetailReadModelExtensions
    {
        public static DetailReadModel ToReadModel(this Detail entity)
        {
            DetailReadModel detailReadModel = new DetailReadModel
            {
                Id = entity.Id,
                Creator = entity.Creator.ToReadModel(),
                CustomDetailText = entity.CustomDetailText,
                DetailTypeId = entity.DetailTypeId,
                DetailType = entity.DetailType.ToReadModel(),
                Editors = entity.Permissions?.Where(x => x.CanEdit).Select(x => x.Member.ToReadModel()).ToList(),
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                Story = entity.Story
            };

            // making the assumption for now that each detail can only be attached to one type of entity...
            string entityTypeCode = entity.DetailAssociations?.Select(x => x.EntityType.Code).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(entityTypeCode))
            {
                if (entityTypeCode == EntityTypeEnum.Group.ToString())
                {
                    Guid entityId = entity.DetailAssociations.First().EntityId;
                    detailReadModel.GroupId = entityId;
                }
                else if (entityTypeCode == EntityTypeEnum.Relationship.ToString())
                {
                    Guid entityId = entity.DetailAssociations.First().EntityId;
                    detailReadModel.RelationshipId = entityId;
                }
                else if (entityTypeCode == EntityTypeEnum.Member.ToString())
                {
                    // can have more than one member associated, but just one group or relationship
                    detailReadModel.MemberIds = entity.DetailAssociations.Select(x => x.EntityId).ToList();
                }
            }

            return detailReadModel;
        }
    }
}