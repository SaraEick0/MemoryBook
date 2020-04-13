namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Business.Relationship.Managers;
    using Business.Relationship.Models;
    using Common.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class RelationshipCommandController : ControllerBase
    {
        private readonly IRelationshipManager manager;

        public RelationshipCommandController(IRelationshipManager manager)
        {
            Contract.RequiresNotNull(manager, nameof(manager));

            this.manager = manager;
        }

        /// <summary>
        /// Creates relationships.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="models">The relationships to create.</param>
        /// <returns>Returns a list of relationship ids.</returns>
        [HttpPost("CreateRelationships")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<Guid>))]
        [SwaggerOperation(OperationId = "CreateRelationships")]
        public async Task<ActionResult> CreateRelationshipsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] CombinedRelationshipCreateModel[] models)
        {
            IList<Guid> relationshipIds = await this.manager.CreateRelationships(memoryBookUniverseId, models).ConfigureAwait(false);

            return this.Ok(relationshipIds);
        }

        /// <summary>
        /// Updates relationships.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="models">The relationships to update.</param>
        /// <returns>Returns a completed task.</returns>
        [HttpPost("UpdateRelationships")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(void))]
        [SwaggerOperation(OperationId = "UpdateRelationships")]
        public async Task<ActionResult> UpdateRelationshipsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] CombinedRelationshipUpdateModel[] models)
        {
            await this.manager.UpdateRelationships(memoryBookUniverseId, models).ConfigureAwait(false);

            return this.Ok();
        }

        /// <summary>
        /// Deletes relationships.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="relationshipIds">The relationship ids to delete.</param>
        /// <returns>Returns a completed task.</returns>
        [HttpPost("DeleteRelationships")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(void))]
        [SwaggerOperation(OperationId = "DeleteRelationships")]
        public async Task<ActionResult> DeleteRelationshipsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] Guid[] relationshipIds)
        {
            await this.manager.DeleteRelationships(memoryBookUniverseId, relationshipIds).ConfigureAwait(false);

            return this.Ok();
        }
    }
}