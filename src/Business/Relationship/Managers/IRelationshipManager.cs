﻿namespace MemoryBook.Business.Relationship.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Repository.RelationshipType;

    public interface IRelationshipManager
    {
        Task<Guid> CreateTwoPersonRelationship(
            Guid memoryBookUniverseId,
            Guid firstMemberId,
            Guid secondMemberId,
            RelationshipTypeEnum firstMemberRelationshipType,
            RelationshipTypeEnum secondMemberRelationshipType,
            DateTime? startDate,
            DateTime? endDate);

        Task<IList<Guid>> CreateRelationships(Guid memoryBookUniverseId, IList<CombinedRelationshipCreateModel> createModels);

        Task UpdateRelationships(
            Guid memoryBookUniverseId,
            IList<CombinedRelationshipUpdateModel> updateModels);

        Task DeleteRelationships(Guid memoryBookUniverseId, IList<Guid> relationshipIds);
    }
}