namespace MemoryBook.Business.Group.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Microsoft.Extensions.Logging;
    using Repository.Group.Managers;
    using Repository.Group.Models;
    using Repository.GroupMembership.Managers;
    using Repository.GroupMembership.Models;
    using Repository.Member.Models;

    public class GroupProvider : IGroupProvider
    {
        private readonly IGroupCommandManager groupCommandManager;
        private readonly IGroupQueryManager groupQueryManager;
        private readonly IGroupMembershipCommandManager groupMembershipCommandManager;
        private readonly ILogger<GroupProvider> logger;

        public GroupProvider(IGroupCommandManager groupCommandManager, IGroupQueryManager groupQueryManager, IGroupMembershipCommandManager groupMembershipCommandManager, ILogger<GroupProvider> logger)
        {
            Contract.RequiresNotNull(groupCommandManager, nameof(groupCommandManager));
            Contract.RequiresNotNull(groupQueryManager, nameof(groupQueryManager));
            Contract.RequiresNotNull(groupMembershipCommandManager, nameof(groupMembershipCommandManager));
            Contract.RequiresNotNull(logger, nameof(logger));

            this.groupCommandManager = groupCommandManager;
            this.groupQueryManager = groupQueryManager;
            this.groupMembershipCommandManager = groupMembershipCommandManager;
            this.logger = logger;
        }

        public async Task<IList<GroupReadModel>> GetAllGroupsAsync(Guid memoryBookUniverseId)
        {
            try
            {
                return await this.groupQueryManager.GetAllGroups(memoryBookUniverseId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"An exception occurred in {nameof(this.GetAllGroupsAsync)}");
                return null;
            }
        }

        public async Task<IList<GroupReadModel>> GetGroupsAsync(Guid memoryBookUniverseId, params Guid[] groupIds)
        {
            try
            {
                return await this.groupQueryManager.GetGroups(memoryBookUniverseId, groupIds).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"An exception occurred in {nameof(this.GetGroupsAsync)}");
                return null;
            }
        }

        public async Task<Guid> CreateGroup(Guid memoryBookUniverseId, string code, string name, string description)
        {
            Contract.RequiresNotNullOrWhitespace(code, nameof(code));
            Contract.RequiresNotNullOrWhitespace(name, nameof(name));

            try
            {
                var allGroups = await this.groupQueryManager.GetAllGroups(memoryBookUniverseId).ConfigureAwait(false);

                if (allGroups.Any(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Group already existed for with code {code} for universe {memoryBookUniverseId}");
                }

                GroupCreateModel groupCreateModel = new GroupCreateModel
                {
                    Name = name,
                    Code = code,
                    Description = description
                };

                var result = await this.groupCommandManager.CreateGroups(memoryBookUniverseId, groupCreateModel)
                    .ConfigureAwait(false);

                if (!result.Success || !result.Ids.Any())
                {
                    return Guid.Empty;
                }

                return result.Ids.First();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"An exception occurred in {nameof(this.CreateGroup)}");
                return Guid.Empty;
            }
        }

        public async Task AddMembersToGroup(Guid memoryBookUniverseId, Guid groupId, IList<MemberReadModel> members)
        {
            Contract.RequiresNotNullOrEmpty(members, nameof(members));

            var groupMemberships = members.Select(x => new GroupMembershipCreateModel { MemberId = x.Id, GroupId = groupId })
                .ToArray();

            await this.groupMembershipCommandManager.CreateGroupMembership(memoryBookUniverseId, groupMemberships)
                .ConfigureAwait(false);
        }
    }
}