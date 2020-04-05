namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Common.Extensions;
    using MemoryBook.Business.MemoryBookUniverse.Managers;
    using Microsoft.AspNetCore.Mvc;
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
            var groupId = await this.memoryBookUniverseManager.CreateUniverse(memoryBookName).ConfigureAwait(false);

            return this.Ok(groupId);
        }
    }
}