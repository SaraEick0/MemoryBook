namespace MemoryBook.Business.Relationship.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Providers;
    using Relationship.Models;
    using Repository.Member.Models;
    using Repository.Relationship.Models;
    using Repository.RelationshipMembership.Models;
    using Repository.RelationshipType;
    using Repository.RelationshipType.Models;

    public class RelationshipManager : IRelationshipManager
    {
        private readonly IRelationshipProvider relationshipProvider;
        private readonly IRelationshipMemberProvider relationshipMemberProvider;
        private readonly IRelationshipTypeProvider relationshipTypeProvider;

        public RelationshipManager(IRelationshipProvider relationshipProvider, IRelationshipMemberProvider relationshipMemberProvider, IRelationshipTypeProvider relationshipTypeProvider)
        {
            Contract.RequiresNotNull(relationshipProvider, nameof(relationshipProvider));
            Contract.RequiresNotNull(relationshipMemberProvider, nameof(relationshipMemberProvider));
            Contract.RequiresNotNull(relationshipTypeProvider, nameof(relationshipTypeProvider));

            this.relationshipProvider = relationshipProvider;
            this.relationshipMemberProvider = relationshipMemberProvider;
            this.relationshipTypeProvider = relationshipTypeProvider;
        }

        public async Task<Guid> CreateTwoPersonRelationship(
            MemberReadModel firstMember,
            MemberReadModel secondMember,
            RelationshipTypeEnum firstMemberRelationshipType,
            RelationshipTypeEnum secondMemberRelationshipType,
            DateTime? startDate,
            DateTime? endDate)
        {
            Contract.RequiresNotNull(firstMember, nameof(firstMember));
            Contract.RequiresNotNull(secondMember, nameof(secondMember));

            RelationshipTypeReadModel firstMemberRelationshipTypeModel = await this.relationshipTypeProvider
                .GetRelationshipType(firstMemberRelationshipType).ConfigureAwait(false);

            RelationshipTypeReadModel secondMemberRelationshipTypeModel = await this.relationshipTypeProvider
                .GetRelationshipType(secondMemberRelationshipType).ConfigureAwait(false);

            var createModel = new CombinedRelationshipCreateModel
            {
                RelationshipMembers = new List<CombinedRelationshipMemberCreateModel>
                {
                    new CombinedRelationshipMemberCreateModel
                    {
                        MemberId = firstMember.Id,
                        MemberRelationshipTypeId = firstMemberRelationshipTypeModel.Id,
                    },
                    new CombinedRelationshipMemberCreateModel
                    {
                        MemberId = secondMember.Id,
                        MemberRelationshipTypeId = secondMemberRelationshipTypeModel.Id
                    }
                },
                StartDate = startDate,
                EndDate = endDate
            };

            return (await this.CreateRelationships(firstMember.MemoryBookUniverseId, new List<CombinedRelationshipCreateModel> { createModel }).ConfigureAwait(false))?.FirstOrDefault() ?? Guid.Empty;
        }

        public async Task<IList<Guid>> CreateRelationships(Guid memoryBookUniverseId, IList<CombinedRelationshipCreateModel> createModels)
        {
            Contract.RequiresNotNull(createModels, nameof(createModels));

            IList<Guid> relationshipIds = new List<Guid>();
            foreach (var createModel in createModels)
            {
                IList<CombinedRelationshipMemberCreateModel> relationshipMembers = createModel.RelationshipMembers;
                Contract.RequiresNotNull(relationshipMembers, nameof(relationshipMembers));

                if (relationshipMembers.Count < 2)
                {
                    throw new ArgumentException("Relationship must have at least two members.");
                }

                Guid relationshipId = await this.relationshipProvider
                    .CreateRelationship(memoryBookUniverseId, createModel.StartDate, createModel.EndDate)
                    .ConfigureAwait(false);

                await this.relationshipMemberProvider
                    .CreateRelationshipMembershipAsync(relationshipId, relationshipMembers, createModel.StartDate,
                        createModel.EndDate)
                    .ConfigureAwait(false);

                relationshipIds.Add(relationshipId);
            }

            return relationshipIds;
        }

        public async Task UpdateRelationships(Guid memoryBookUniverseId, IList<CombinedRelationshipUpdateModel> updateModels)
        {
            Contract.RequiresNotNull(updateModels, nameof(updateModels));

            foreach (var updateModel in updateModels)
            {
                IList<CombinedRelationshipMemberCreateModel> inputModelRelationshipMembers = updateModel.RelationshipMembers;
                Contract.RequiresNotNull(inputModelRelationshipMembers, nameof(inputModelRelationshipMembers));

                if (inputModelRelationshipMembers.Count < 2)
                {
                    throw new ArgumentException("Relationship must have at least two members.");
                }

                RelationshipReadModel relationship = (await this.relationshipProvider
                    .GetRelationships(memoryBookUniverseId, new List<Guid> { updateModel.Id })
                    .ConfigureAwait(false))?.FirstOrDefault();

                if (relationship == null)
                {
                    throw new InvalidOperationException($"Relationship with id {updateModel.Id} did not exist!");
                }

                await this.relationshipProvider.UpdateRelationship(memoryBookUniverseId, updateModel.Id, updateModel.StartDate, updateModel.EndDate)
                    .ConfigureAwait(false);

                IList<RelationshipMembershipReadModel> existingMembers = await this.relationshipMemberProvider
                    .GetRelationshipMembershipsForRelationshipAsync(memoryBookUniverseId, updateModel.Id)
                    .ConfigureAwait(false);

                List<Guid> existingMemberIds = existingMembers.Select(x => x.MemberId).ToList();
                List<Guid> inputModelMemberIds = inputModelRelationshipMembers.Select(x => x.MemberId).ToList();

                List<CombinedRelationshipMemberCreateModel> newMembers = inputModelRelationshipMembers.Where(x => !existingMemberIds.Contains(x.MemberId)).ToList();
                List<RelationshipMembershipReadModel> membersToUpdate = existingMembers.Where(x => inputModelMemberIds.Contains(x.MemberId)).ToList();
                List<RelationshipMembershipReadModel> membersToDelete = existingMembers.Where(x => !inputModelMemberIds.Contains(x.MemberId)).ToList();

                if (newMembers.Any())
                {
                    await this.relationshipMemberProvider
                        .CreateRelationshipMembershipAsync(updateModel.Id, newMembers, updateModel.StartDate,
                            updateModel.EndDate)
                        .ConfigureAwait(false);
                }

                if (membersToUpdate.Any())
                {
                    await this.relationshipMemberProvider
                        .UpdateRelationshipMembershipAsync(memoryBookUniverseId, membersToUpdate, updateModel.StartDate, updateModel.EndDate)
                        .ConfigureAwait(false);
                }

                if (membersToDelete.Any())
                {
                    await this.relationshipMemberProvider
                        .DeleteRelationshipMembershipAsync(memoryBookUniverseId, membersToDelete)
                        .ConfigureAwait(false);
                }
            }
        }

        public async Task DeleteRelationships(Guid memoryBookUniverseId, IList<Guid> relationshipIds)
        {
            Contract.RequiresNotNull(relationshipIds, nameof(relationshipIds));

            foreach (var relationshipId in relationshipIds)
            {
                RelationshipReadModel relationship = (await this.relationshipProvider
                    .GetRelationships(memoryBookUniverseId, new List<Guid> { relationshipId })
                    .ConfigureAwait(false))?.FirstOrDefault();

                if (relationship == null)
                {
                    throw new InvalidOperationException($"Relationship with id {relationshipId} did not exist!");
                }

                IList<RelationshipMembershipReadModel> existingMembers = await this.relationshipMemberProvider
                    .GetRelationshipMembershipsForRelationshipAsync(memoryBookUniverseId, relationshipId)
                    .ConfigureAwait(false);

                if (existingMembers.Any())
                {
                    await this.relationshipMemberProvider
                        .DeleteRelationshipMembershipAsync(memoryBookUniverseId, existingMembers)
                        .ConfigureAwait(false);
                }

                await this.relationshipProvider.DeleteRelationships(memoryBookUniverseId, relationshipId)
                    .ConfigureAwait(false);
            }
        }
    }
}