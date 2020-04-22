namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Business.Relationship.Providers;
    using Common.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Repository.Relationship.Models;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class RelationshipQueryController : ControllerBase
    {
        private readonly IRelationshipProvider relationshipProvider;

        public RelationshipQueryController(IRelationshipProvider relationshipProvider)
        {
            Contract.RequiresNotNull(relationshipProvider, nameof(relationshipProvider));

            this.relationshipProvider = relationshipProvider;
        }

        /// <summary>
        /// Gets relationships by id.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="relationshipIds">The relationship ids.</param>
        /// <returns>Returns relationships.</returns>
        [HttpPost("GetRelationshipsById")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<RelationshipReadModel>))]
        [SwaggerOperation(OperationId = "GetRelationshipsById")]
        public async Task<ActionResult> GetRelationshipsByIdAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] IEnumerable<Guid> relationshipIds)
        {
            IList<RelationshipReadModel> relationships = await this.relationshipProvider.GetRelationships(memoryBookUniverseId, relationshipIds?.ToList()).ConfigureAwait(false);

            return this.Ok(relationships);
        }
    }
}