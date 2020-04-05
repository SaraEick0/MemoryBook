namespace MemoryBook.Business.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Extensions;
    using Microsoft.Extensions.Logging;
    using Repository.Group.Managers;
    using Repository.Group.Models;
    using Repository.GroupMembership.Managers;
    using Repository.GroupMembership.Models;
    using Repository.Member.Models;

    public class GroupManager : IGroupManager
    {
        private readonly IGroupCommandManager groupCommandManager;
        private readonly IGroupQueryManager groupQueryManager;
        private readonly IGroupMembershipCommandManager groupMembershipCommandManager;
        private readonly ILogger<GroupManager> logger;

        public GroupManager(IGroupCommandManager groupCommandManager, IGroupQueryManager groupQueryManager, IGroupMembershipCommandManager groupMembershipCommandManager, ILogger<GroupManager> logger)
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

        public async Task<GroupReadModel> CreateGroup(Guid memoryBookUniverseId, string code, string name, string description)
        {
            Contract.RequiresNotNullOrWhitespace(code, nameof(code));
            Contract.RequiresNotNullOrWhitespace(name, nameof(name));

            try
            {
                var allGroups = await this.groupQueryManager.GetAllGroups(memoryBookUniverseId).ConfigureAwait(false);

                if (allGroups.Any(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException(
                        $"Group already existed for with code {code} for universe {memoryBookUniverseId}");
                }

                GroupCreateModel groupCreateModel = new GroupCreateModel
                {
                    Name = name,
                    Code = code,
                    Description = description
                };

                return await this.CreateGroup(memoryBookUniverseId, groupCreateModel)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"An exception occurred in {nameof(this.CreateGroup)}");
                return null;
            }
        }

        public async Task AddMembersToGroup(Guid memoryBookUniverseId, GroupReadModel group, IList<MemberReadModel> members)
        {
            Contract.RequiresNotNull(group, nameof(group));
            Contract.RequiresNotNullOrEmpty(members, nameof(members));

            var groupMemberships = members.Select(x => new GroupMembershipCreateModel { MemberId = x.Id, GroupId = group.Id })
                .ToArray();

            await this.groupMembershipCommandManager.CreateGroupMembership(memoryBookUniverseId, groupMemberships)
                .ConfigureAwait(false);

            foreach (var member in members)
            {
                group.AddMember(member);
            }
        }

        private async Task<GroupReadModel> CreateGroup(Guid memoryBookUniverseId, GroupCreateModel groupCreateModel)
        {
            var result = await this.groupCommandManager.CreateGroups(memoryBookUniverseId, groupCreateModel)
                .ConfigureAwait(false);

            if (!result.Success || !result.Ids.Any())
            {
                return null;
            }

            var groupReadModel = await this.groupQueryManager.GetGroups(memoryBookUniverseId, result.Ids)
                .ConfigureAwait(false);

            return groupReadModel.FirstOrDefault();
        }
    }
}