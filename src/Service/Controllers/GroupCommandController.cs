namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Common.Extensions;
    using MemoryBook.Repository.Common.Models;
    using MemoryBook.Repository.Group.Managers;
    using Microsoft.AspNetCore.Mvc;
    using Repository.Group.Models;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class GroupCommandController : ControllerBase
    {
        private readonly IGroupCommandManager manager;

        public GroupCommandController(IGroupCommandManager manager)
        {
            Contract.RequiresNotNull(manager, nameof(manager));

            this.manager = manager;
        }

        /// <summary>
        /// Creates groups.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="models">The groups to create.</param>
        /// <returns>Returns a response model.</returns>
        [HttpPost("CreateGroups")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(MemoryBookResponseModel))]
        [SwaggerOperation(OperationId = "CreateGroups")]
        public async Task<ActionResult> CreateGroupsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] GroupCreateModel[] models)
        {
            MemoryBookResponseModel memoryBookResponseModel = await this.manager.CreateGroups(memoryBookUniverseId, models).ConfigureAwait(false);

            return this.Ok(memoryBookResponseModel);
        }

        /// <summary>
        /// Updates groups.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="models">The groups to update.</param>
        /// <returns>Returns a completed task.</returns>
        [HttpPost("UpdateGroups")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(void))]
        [SwaggerOperation(OperationId = "UpdateGroups")]
        public async Task<ActionResult> UpdateGroupsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] GroupUpdateModel[] models)
        {
            await this.manager.UpdateGroups(memoryBookUniverseId, models).ConfigureAwait(false);

            return this.Ok();
        }

        /// <summary>
        /// Deletes groups.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="groupIds">The group ids to delete.</param>
        /// <returns>Returns a completed task.</returns>
        [HttpPost("DeleteGroups")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(void))]
        [SwaggerOperation(OperationId = "DeleteGroups")]
        public async Task<ActionResult> DeleteGroupsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] Guid[] groupIds)
        {
            await this.manager.DeleteGroups(memoryBookUniverseId, groupIds).ConfigureAwait(false);

            return this.Ok();
        }
    }
}