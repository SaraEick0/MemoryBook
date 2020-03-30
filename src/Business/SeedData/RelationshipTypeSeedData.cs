namespace MemoryBook.Business.SeedData
{
    using System;
    using System.Collections.Generic;
    using RelationshipType;
    using RelationshipType.Models;

    public static class RelationshipTypeSeedData
    {
        public static IList<RelationshipTypeCreateModel> RelationshipTypes
        {
            get
            {
                IList<RelationshipTypeCreateModel> createModels = new List<RelationshipTypeCreateModel>();

                foreach (var relationshipTypeCode in Enum.GetValues(typeof(RelationshipTypeEnum)))
                {
                    createModels.Add(new RelationshipTypeCreateModel { Code = relationshipTypeCode.ToString() });
                }

                return createModels;
            }
        }
    }
} 