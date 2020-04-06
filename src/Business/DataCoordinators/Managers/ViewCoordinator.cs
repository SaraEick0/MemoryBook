using System;
using System.Collections.Generic;

namespace MemoryBook.Business.DataCoordinators.Managers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Detail.Extensions;
    using Detail.Models;
    using Detail.Providers;
    using Group.Extensions;
    using Group.Models;
    using Group.Providers;
    using Member.Extensions;
    using Member.Models;
    using Member.Providers;
    using Microsoft.Extensions.Logging;
    using Relationship.Extensions;
    using Relationship.Providers;
    using Repository.DetailType.Managers;
    using Repository.DetailType.Models;
    using Repository.Group.Models;
    using Repository.Member.Models;
    using Repository.RelationshipType.Models;

    public class ViewCoordinator : IViewCoordinator
    {
        private readonly IGroupProvider groupProvider;
        private readonly IMemberProvider memberProvider;
        private readonly IRelationshipProvider relationshipProvider;
        private readonly IRelationshipTypeProvider relationshipTypeProvider;
        private readonly IRelationshipMemberProvider relationshipMemberProvider;
        private readonly IDetailProvider detailProvider;
        private readonly IDetailTypeQueryManager detailTypeQueryManager;
        private readonly ILogger<ViewCoordinator> logger;

        public ViewCoordinator(
            IGroupProvider groupProvider,
            IMemberProvider memberProvider,
            IRelationshipProvider relationshipProvider,
            IRelationshipMemberProvider relationshipMemberProvider,
            IRelationshipTypeProvider relationshipTypeProvider,
            IDetailProvider detailProvider,
            IDetailTypeQueryManager detailTypeQueryManager,
            ILogger<ViewCoordinator> logger)
        {
            Contract.RequiresNotNull(groupProvider, nameof(groupProvider));
            Contract.RequiresNotNull(memberProvider, nameof(memberProvider));
            Contract.RequiresNotNull(relationshipProvider, nameof(relationshipProvider));
            Contract.RequiresNotNull(relationshipMemberProvider, nameof(relationshipMemberProvider));
            Contract.RequiresNotNull(relationshipTypeProvider, nameof(relationshipTypeProvider));
            Contract.RequiresNotNull(detailProvider, nameof(detailProvider));
            Contract.RequiresNotNull(detailTypeQueryManager, nameof(detailTypeQueryManager));
            Contract.RequiresNotNull(logger, nameof(logger));

            this.groupProvider = groupProvider;
            this.memberProvider = memberProvider;
            this.relationshipProvider = relationshipProvider;
            this.relationshipMemberProvider = relationshipMemberProvider;
            this.relationshipTypeProvider = relationshipTypeProvider;
            this.detailProvider = detailProvider;
            this.detailTypeQueryManager = detailTypeQueryManager;
            this.logger = logger;
        }

        public async Task<GroupViewModel> GetGroupViewModel(Guid memoryBookUniverseId, Guid groupId)
        {
            IList<GroupReadModel> groups = await this.groupProvider.GetGroupsAsync(memoryBookUniverseId, groupId).ConfigureAwait(false);

            if (groups == null || groups.Count == 0)
            {
                return null;
            }

            GroupReadModel groupReadModel = groups.First();
            GroupViewModel viewModel = groupReadModel.ToViewModel();

            IList<DetailTypeReadModel> detailTypes = await this.detailTypeQueryManager.GetAllDetailTypes().ConfigureAwait(false);

            Dictionary<Guid, DetailTypeReadModel> detailTypesDictionary = detailTypes.ToDictionary(x => x.Id);

            Task<IList<DetailsByEntityModel>> groupDetails = this.detailProvider.GetDetailsForGroups(memoryBookUniverseId, groupId);
            var memberViewModelsTask = this.GetMemberViewModels(memoryBookUniverseId, groupReadModel.MemberIds, detailTypesDictionary);

            await Task.WhenAll(groupDetails, memberViewModelsTask).ConfigureAwait(false);

            viewModel.Details = groupDetails.Result?.FirstOrDefault()?.Details
                .Select(x => x.ToViewModel(detailTypesDictionary)).ToList();

            viewModel.Members = memberViewModelsTask.Result;

            return viewModel;
        }

        private async Task<IList<MemberViewModel>> GetMemberViewModels(Guid memoryBookUniverseId, IList<Guid> memberIds, IDictionary<Guid, DetailTypeReadModel> detailTypesDictionary)
        {
            try
            {
                var members = await this.memberProvider.GetMembers(memoryBookUniverseId, memberIds).ConfigureAwait(false);
                var relationshipTypes = await relationshipTypeProvider.GetAllRelationshipTypes().ConfigureAwait(false);

                Dictionary<Guid, MemberReadModel> memberDictionary = members.ToDictionary(x => x.Id);
                Dictionary<Guid, RelationshipTypeReadModel> relationshipTypeDictionary =
                    relationshipTypes.ToDictionary(x => x.Id);

                List<Guid> distinctRelationshipIds = members.SelectMany(x => x.RelationshipIds).Distinct().ToList();

                var relationships = await relationshipProvider.GetRelationships(memoryBookUniverseId, distinctRelationshipIds)
                    .ConfigureAwait(false);

                var memberToRelationshipMembershipDictionary = await this.relationshipMemberProvider
                    .GetRelationshipMembershipsForMembersAsync(memoryBookUniverseId, memberIds);

                var memberDetails = await this.detailProvider.GetDetailsForMembers(memoryBookUniverseId, memberIds.ToArray());
                var relationshipDetails = await this.detailProvider.GetDetailsForRelationships(memoryBookUniverseId,
                        distinctRelationshipIds.ToArray());

                var relationshipDetailsDictionary = relationshipDetails.ToDictionary(x => x.EntityId,
                    x => x.Details.Select(m => m.ToViewModel(detailTypesDictionary)).ToList());

                var relationshipDictionary = relationships.Select(x => x.ToViewModel(memberDictionary,
                        memberToRelationshipMembershipDictionary, relationshipTypeDictionary, relationshipDetailsDictionary))
                    .ToList();

                var memberDetailsDictionary = memberDetails.ToDictionary(x => x.EntityId,
                    x => x.Details.Select(m => m.ToViewModel(detailTypesDictionary)).ToList());
                return members.Select(x => x.ToViewModel(memberDetailsDictionary, relationshipDictionary)).ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"An exception has occurred in {nameof(ViewCoordinator)} while retrieving member view models.");
                return null;
            }
        }
    }
}