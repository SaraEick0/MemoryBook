namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Common.Extensions;
    using MemoryBook.Repository.MemoryBookUniverse.Managers;
    using MemoryBook.Repository.MemoryBookUniverse.Models;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class MemoryBookUniverseQueryController : ControllerBase
    {
        private readonly IMemoryBookUniverseQueryManager memoryBookUniverseQueryManager;

        public MemoryBookUniverseQueryController(IMemoryBookUniverseQueryManager memoryBookUniverseQueryManager)
        {
            Contract.RequiresNotNull(memoryBookUniverseQueryManager, nameof(memoryBookUniverseQueryManager));

            this.memoryBookUniverseQueryManager = memoryBookUniverseQueryManager;
        }

        /// <summary>
        /// Gets all memory book universes.
        /// </summary>
        /// <returns>Returns all memory book universes.</returns>
        [HttpPost("GetAllMemoryBookUniverses")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(MemoryBookUniverseReadModel))]
        [SwaggerOperation(OperationId = "GetAllMemoryBookUniverses")]
        public async Task<ActionResult> GetAllMemoryBookUniversesAsync()
        {
            IList<MemoryBookUniverseReadModel> memoryBooks = await this.memoryBookUniverseQueryManager.GetAllMemoryBookUniverses().ConfigureAwait(false);

            return this.Ok(memoryBooks);
        }

        /// <summary>
        /// Gets memory book universes by id.
        /// </summary>
        /// <param name="memoryBookUniverseIds">The universe ids.</param>
        /// <returns>Returns memory book universes.</returns>
        [HttpPost("GetMemoryBookUniversesById")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(MemoryBookUniverseReadModel))]
        [SwaggerOperation(OperationId = "GetMemoryBookUniversesById")]
        public async Task<ActionResult> GetMemoryBookUniversesByIdAsync([FromQuery] Guid[] memoryBookUniverseIds)
        {
            IList<MemoryBookUniverseReadModel> memoryBooks = await this.memoryBookUniverseQueryManager.GetMemoryBookUniverses(memoryBookUniverseIds).ConfigureAwait(false);

            return this.Ok(memoryBooks);
        }

        /// <summary>
        /// Gets memory book universes by name.
        /// </summary>
        /// <param name="memoryBookNames">The universe names.</param>
        /// <returns>Returns memory book universes.</returns>
        [HttpPost("GetMemoryBookUniversesByName")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(MemoryBookUniverseReadModel))]
        [SwaggerOperation(OperationId = "GetMemoryBookUniversesByName")]
        public async Task<ActionResult> GetMemoryBookUniversesByNameAsync([FromQuery] string[] memoryBookNames)
        {
            IList<MemoryBookUniverseReadModel> memoryBooks = await this.memoryBookUniverseQueryManager.GetMemoryBookUniverses(memoryBookNames).ConfigureAwait(false);

            return this.Ok(memoryBooks);
        }
    }
}