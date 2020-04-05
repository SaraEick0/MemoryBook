namespace MemoryBook.Repository.SeedData
{
    using System;
    using System.Collections.Generic;
    using EntityType;
    using EntityType.Models;

    public static class EntityTypeSeedData
    {
        public static IList<EntityTypeCreateModel> EntityTypes
        {
            get
            {
                IList<EntityTypeCreateModel> createModels = new List<EntityTypeCreateModel>();

                foreach (var entityTypeCode in Enum.GetValues(typeof(EntityTypeEnum)))
                {
                    createModels.Add(new EntityTypeCreateModel { Code = entityTypeCode.ToString() });
                }

                return createModels;
            }
        }
    }
}