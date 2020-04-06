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
    }
}