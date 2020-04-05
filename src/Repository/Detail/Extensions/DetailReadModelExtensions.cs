namespace MemoryBook.Repository.Detail.Extensions
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
            DetailReadModel detailReadModel = entity.ToShallowReadModel();

            detailReadModel.Creator = entity.Creator.ToShallowReadModel();
            detailReadModel.DetailType = entity.DetailType.ToReadModel();
            detailReadModel.Editors = entity.Permissions?.Where(x => x.CanEdit).Select(x => x.Member.ToShallowReadModel())
                .ToList();

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

        public static DetailReadModel ToShallowReadModel(this Detail entity)
        {
            DetailReadModel detailReadModel = new DetailReadModel
            {
                Id = entity.Id,
                CreatorId = entity.CreatorId,
                CustomDetailText = entity.CustomDetailText,
                DetailTypeId = entity.DetailTypeId,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                Story = entity.Story
            };

            return detailReadModel;
        }
    }
}