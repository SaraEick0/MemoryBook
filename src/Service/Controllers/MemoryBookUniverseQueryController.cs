namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Business.MemoryBookUniverse.Models;
    using Common.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Repository.MemoryBookUniverse.Managers;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class MemoryBookUniverseQueryController : ControllerBase
    {
        private readonly IMemoryBookUniverseManager memoryBookUniverseManager;

        public MemoryBookUniverseQueryController(IMemoryBookUniverseManager memoryBookUniverseManager)
        {
            Contract.RequiresNotNull(memoryBookUniverseManager, nameof(memoryBookUniverseManager));

            this.memoryBookUniverseManager = memoryBookUniverseManager;
        }

        /// <summary>
        /// Gets a memory book universe by id.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <returns>Returns a memory book universe.</returns>
        [HttpPost("GetMemoryBookUniverseById")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(MemoryBookUniverseReadModel))]
        [SwaggerOperation(OperationId = "GetMemoryBookUniverseById")]
        public async Task<ActionResult> GetMemoryBookUniverseByIdAsync([FromQuery] Guid memoryBookUniverseId)
        {
            MemoryBookUniverseReadModel memoryBook = await this.memoryBookUniverseManager.GetUniverse(memoryBookUniverseId).ConfigureAwait(false);

            return this.Ok(memoryBook);
        }

        /// <summary>
        /// Gets a memory book universe by name.
        /// </summary>
        /// <param name="memoryBookName">The universe name.</param>
        /// <returns>Returns a memory book universe.</returns>
        [HttpPost("GetMemoryBookUniverseByName")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(MemoryBookUniverseReadModel))]
        [SwaggerOperation(OperationId = "GetMemoryBookUniverseByName")]
        public async Task<ActionResult> GetMemoryBookUniverseByNameAsync([FromQuery] string memoryBookName)
        {
            MemoryBookUniverseReadModel memoryBook = await this.memoryBookUniverseManager.GetUniverse(memoryBookName).ConfigureAwait(false);

            return this.Ok(memoryBook);
        }
    }
}