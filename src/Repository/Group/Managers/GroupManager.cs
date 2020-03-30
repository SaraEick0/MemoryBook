namespace MemoryBook.Repository.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.Group.Managers;
    using Business.Group.Models;
    using Business.GroupMembership.Managers;
    using Business.GroupMembership.Models;
    using Business.Member.Models;

    public class GroupManager : IGroupManager
    {
        private readonly IGroupCommandManager groupCommandManager;
        private readonly IGroupQueryManager groupQueryManager;
        private readonly IGroupMembershipCommandManager groupMembershipCommandManager;

        public GroupManager(IGroupCommandManager groupCommandManager, IGroupQueryManager groupQueryManager, IGroupMembershipCommandManager groupMembershipCommandManager)
        {
            this.groupCommandManager = groupCommandManager ?? throw new ArgumentNullException(nameof(groupCommandManager));
            this.groupQueryManager = groupQueryManager ?? throw new ArgumentNullException(nameof(groupQueryManager));
            this.groupMembershipCommandManager = groupMembershipCommandManager ?? throw new ArgumentNullException(nameof(groupMembershipCommandManager));
        }

        public async Task<GroupReadModel> CreateGroup(Guid memoryBookUniverseId, string code, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException(nameof(code));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

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
                return null;
            }
        }

        public async Task AddMembersToGroup(Guid memoryBookUniverseId, GroupReadModel group, IList<MemberReadModel> members)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (members == null || members.Count == 0)
            {
                throw new ArgumentNullException(nameof(members));
            }

            var groupMemberships = members.Select(x => new GroupMembershipCreateModel { MemberId = x.Id, GroupId = group.Id })
                .ToArray();

            await this.groupMembershipCommandManager.CreateGroupMembership(memoryBookUniverseId, groupMemberships)
                .ConfigureAwait(false);
        }

        private async Task<GroupReadModel> CreateGroup(Guid memoryBookUniverseId, GroupCreateModel groupCreateModel)
        {
            var id = await this.groupCommandManager.CreateGroups(memoryBookUniverseId, groupCreateModel)
                .ConfigureAwait(false);

            if (id == null || id.Count == 0)
            {
                return null;
            }

            var groupReadModel = await this.groupQueryManager.GetGroups(memoryBookUniverseId, id)
                .ConfigureAwait(false);

            return groupReadModel.FirstOrDefault();
        }
    }
}