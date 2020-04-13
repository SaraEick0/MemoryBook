namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Common.Extensions;
    using MemoryBook.Business.MemoryBookUniverse.Managers;
    using Microsoft.AspNetCore.Mvc;
    using Repository.MemoryBookUniverse.Models;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class MemoryBookUniverseCommandController : ControllerBase
    {
        private readonly IMemoryBookUniverseManager memoryBookUniverseManager;

        public MemoryBookUniverseCommandController(IMemoryBookUniverseManager memoryBookUniverseManager)
        {
            Contract.RequiresNotNull(memoryBookUniverseManager, nameof(memoryBookUniverseManager));

            this.memoryBookUniverseManager = memoryBookUniverseManager;
        }

        /// <summary>
        /// Creates a memory book universe.
        /// </summary>
        /// <param name="memoryBookName">The universe name.</param>
        /// <returns>Returns a memory book universe id.</returns>
        [HttpPost("CreateMemoryBookUniverse")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(Guid))]
        [SwaggerOperation(OperationId = "CreateMemoryBookUniverse")]
        public async Task<ActionResult> CreateMemoryBookUniverseAsync([FromQuery] string memoryBookName)
        {
            var memoryBookId = await this.memoryBookUniverseManager.CreateUniverse(memoryBookName).ConfigureAwait(false);

            return this.Ok(memoryBookId);
        }

        /// <summary>
        /// Updates memory book universes.
        /// </summary>
        /// <param name="updateModels">The universe update models.</param>
        /// <returns>A completed task.</returns>
        [HttpPost("UpdateMemoryBookUniverses")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(void))]
        [SwaggerOperation(OperationId = "UpdateMemoryBookUniverses")]
        public async Task<ActionResult> UpdateMemoryBookUniversesAsync([FromBody] params MemoryBookUniverseUpdateModel[] updateModels)
        {
            await this.memoryBookUniverseManager.UpdateUniverses(updateModels).ConfigureAwait(false);

            return this.Ok();
        }

        /// <summary>
        /// Deletes memory book universes.
        /// </summary>
        /// <param name="memoryBookIds">The universe ids.</param>
        [HttpPost("DeleteMemoryBookUniverses")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(Guid))]
        [SwaggerOperation(OperationId = "DeleteMemoryBookUniverses")]
        public async Task<ActionResult> DeleteMemoryBookUniverseAsync([FromQuery] params Guid[] memoryBookIds)
        {
            await this.memoryBookUniverseManager.DeleteUniverse(memoryBookIds).ConfigureAwait(false);

            return this.Ok();
        }
    }
}