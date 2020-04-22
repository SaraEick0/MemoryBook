namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Business.Detail.Models;
    using Business.Detail.Providers;
    using Common.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class DetailQueryController : ControllerBase
    {
        private readonly IDetailProvider detailProvider;

        public DetailQueryController(IDetailProvider detailProvider)
        {
            Contract.RequiresNotNull(detailProvider, nameof(detailProvider));

            this.detailProvider = detailProvider;
        }

        /// <summary>
        /// Gets details for members.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="memberIds">The member ids.</param>
        /// <returns>Returns details for given members.</returns>
        [HttpPost("GetDetailsForMembers")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<DetailsByEntityModel>))]
        [SwaggerOperation(OperationId = "GetDetailsForMembers")]
        public async Task<ActionResult> GetDetailsForMembersAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] Guid[] memberIds)
        {
            IList<DetailsByEntityModel> details = await this.detailProvider.GetDetailsForMembers(memoryBookUniverseId, memberIds).ConfigureAwait(false);

            return this.Ok(details);
        }

        /// <summary>
        /// Gets details for groups.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="groupIds">The group ids.</param>
        /// <returns>Returns details for given groups.</returns>
        [HttpPost("GetDetailsForGroups")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<DetailsByEntityModel>))]
        [SwaggerOperation(OperationId = "GetDetailsForGroups")]
        public async Task<ActionResult> GetDetailsForGroupsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] Guid[] groupIds)
        {
            IList<DetailsByEntityModel> details = await this.detailProvider.GetDetailsForGroups(memoryBookUniverseId, groupIds).ConfigureAwait(false);

            return this.Ok(details);
        }

        /// <summary>
        /// Gets details for relationships.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="relationshipIds">The relationship ids.</param>
        /// <returns>Returns details for given relationships.</returns>
        [HttpPost("GetDetailsForRelationships")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<DetailsByEntityModel>))]
        [SwaggerOperation(OperationId = "GetDetailsForRelationships")]
        public async Task<ActionResult> GetDetailsForRelationshipsAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] Guid[] relationshipIds)
        {
            IList<DetailsByEntityModel> details = await this.detailProvider.GetDetailsForRelationships(memoryBookUniverseId, relationshipIds).ConfigureAwait(false);

            return this.Ok(details);
        }
    }
}