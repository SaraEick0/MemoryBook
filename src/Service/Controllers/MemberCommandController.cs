namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Common.Extensions;
    using MemoryBook.Repository.Member.Managers;
    using Microsoft.AspNetCore.Mvc;
    using Repository.Member.Models;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class MemberCommandController : ControllerBase
    {
        private readonly IMemberCommandManager manager;

        public MemberCommandController(IMemberCommandManager manager)
        {
            Contract.RequiresNotNull(manager, nameof(manager));

            this.manager = manager;
        }

        /// <summary>
        /// Creates members.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="models">The members to create.</param>
        /// <returns>Returns a list of member ids.</returns>
        [HttpPost("CreateMembers")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<Guid>))]
        [SwaggerOperation(OperationId = "CreateMembers")]
        public async Task<ActionResult> CreateMembersAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] MemberCreateModel[] models)
        {
            IList<Guid> memberIds = await this.manager.CreateMembers(memoryBookUniverseId, models).ConfigureAwait(false);

            return this.Ok(memberIds);
        }

        /// <summary>
        /// Updates members.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="models">The members to update.</param>
        /// <returns>Returns a completed task.</returns>
        [HttpPost("UpdateMembers")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(void))]
        [SwaggerOperation(OperationId = "UpdateMembers")]
        public async Task<ActionResult> UpdateMembersAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] MemberUpdateModel[] models)
        {
            await this.manager.UpdateMembers(memoryBookUniverseId, models).ConfigureAwait(false);

            return this.Ok();
        }

        /// <summary>
        /// Deletes members.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="memberIds">The member ids to delete.</param>
        /// <returns>Returns a completed task.</returns>
        [HttpPost("DeleteMembers")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(void))]
        [SwaggerOperation(OperationId = "DeleteMembers")]
        public async Task<ActionResult> DeleteMembersAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] Guid[] memberIds)
        {
            await this.manager.DeleteMembers(memoryBookUniverseId, memberIds).ConfigureAwait(false);

            return this.Ok();
        }
    }
}