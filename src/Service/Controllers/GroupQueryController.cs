namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Business.Group.Models;
    using Common.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Repository.Group.Managers;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class GroupQueryController : ControllerBase
    {
        private readonly IGroupViewCoordinator groupViewCoordinator;

        public GroupQueryController(IGroupViewCoordinator groupViewCoordinator)
        {
            Contract.RequiresNotNull(groupViewCoordinator, nameof(groupViewCoordinator));

            this.groupViewCoordinator = groupViewCoordinator;
        }

        /// <summary>
        /// Gets all groups for a given universe.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <returns>Returns a list of groups.</returns>
        [HttpPost("GetAllGroups")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IEnumerable<GroupReadModel>))]
        [SwaggerOperation(OperationId = "GetAllGroups")]
        public async Task<ActionResult> GetAllGroupsAsync([FromQuery] Guid memoryBookUniverseId)
        {
            var groups = await this.groupViewCoordinator.GetAllGroupsAsync(memoryBookUniverseId).ConfigureAwait(false);

            return this.Ok(groups);
        }
    }
}