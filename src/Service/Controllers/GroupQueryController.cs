namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Business.DataCoordinators.Managers;
    using Business.Group.Models;
    using Business.Group.Providers;
    using Common.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Repository.Group.Models;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class GroupQueryController : ControllerBase
    {
        private readonly IViewCoordinator groupViewCoordinator;
        private readonly IGroupProvider groupProvider;

        public GroupQueryController(IViewCoordinator groupViewCoordinator, IGroupProvider groupProvider)
        {
            Contract.RequiresNotNull(groupViewCoordinator, nameof(groupViewCoordinator));
            Contract.RequiresNotNull(groupProvider, nameof(groupProvider));

            this.groupViewCoordinator = groupViewCoordinator;
            this.groupProvider = groupProvider;
        }

        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <returns>Returns all groups.</returns>
        [HttpPost("GetAllGroups")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<GroupReadModel>))]
        [SwaggerOperation(OperationId = "GetAllGroups")]
        public async Task<ActionResult> GetAllGroupsAsync([FromQuery] Guid memoryBookUniverseId)
        {
            IList<GroupReadModel> groups = await this.groupProvider.GetAllGroupsAsync(memoryBookUniverseId).ConfigureAwait(false);

            return this.Ok(groups);
        }

        /// <summary>
        /// Gets a group by id.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="groupId">The group id.</param>
        /// <returns>Returns a group.</returns>
        [HttpPost("GetGroupById")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(GroupViewModel))]
        [SwaggerOperation(OperationId = "GetGroupById")]
        public async Task<ActionResult> GetGroupByIdAsync([FromQuery] Guid memoryBookUniverseId, [FromQuery] Guid groupId)
        {
            GroupViewModel group = await this.groupViewCoordinator.GetGroupViewModel(memoryBookUniverseId, groupId).ConfigureAwait(false);

            return this.Ok(group);
        }
    }
}