namespace MemoryBook.Repository.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Business.Group.Managers;
    using Business.Group.Models;
    using Common.Extensions;
    using Microsoft.Extensions.Logging;

    public class GroupViewCoordinator : IGroupViewCoordinator
    {
        private readonly IGroupQueryManager groupQueryManager;
        private readonly ILogger<GroupViewCoordinator> logger;

        public GroupViewCoordinator(IGroupQueryManager groupQueryManager, ILogger<GroupViewCoordinator> logger)
        {
            Contract.RequiresNotNull(groupQueryManager, nameof(groupQueryManager));
            Contract.RequiresNotNull(logger, nameof(logger));

            this.groupQueryManager = groupQueryManager;
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
    }
}