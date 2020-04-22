namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Common.Extensions;
    using MemoryBook.Repository.Detail.Managers;
    using Microsoft.AspNetCore.Mvc;
    using Repository.Detail.Models;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class DetailCommandController : ControllerBase
    {
        private readonly IDetailCommandManager manager;

        public DetailCommandController(IDetailCommandManager manager)
        {
            Contract.RequiresNotNull(manager, nameof(manager));

            this.manager = manager;
        }

        /// <summary>
        /// Creates details.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="models">The details to create.</param>
        /// <returns>Returns a list of detail ids.</returns>
        [HttpPost("CreateDetails")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<Guid>))]
        [SwaggerOperation(OperationId = "CreateDetails")]
        public async Task<ActionResult> CreateDetailsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] DetailCreateModel[] models)
        {
            IList<Guid> detailIds = await this.manager.CreateDetails(memoryBookUniverseId, models).ConfigureAwait(false);

            return this.Ok(detailIds);
        }

        /// <summary>
        /// Updates details.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="models">The details to update.</param>
        /// <returns>Returns a completed task.</returns>
        [HttpPost("UpdateDetails")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(void))]
        [SwaggerOperation(OperationId = "UpdateDetails")]
        public async Task<ActionResult> UpdateDetailsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] DetailUpdateModel[] models)
        {
            await this.manager.UpdateDetails(memoryBookUniverseId, models).ConfigureAwait(false);

            return this.Ok();
        }

        /// <summary>
        /// Deletes details.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="detailIds">The detail ids to delete.</param>
        /// <returns>Returns a completed task.</returns>
        [HttpPost("DeleteDetails")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(void))]
        [SwaggerOperation(OperationId = "DeleteDetails")]
        public async Task<ActionResult> DeleteDetailsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] Guid[] detailIds)
        {
            await this.manager.DeleteDetails(memoryBookUniverseId, detailIds).ConfigureAwait(false);

            return this.Ok();
        }
    }
}