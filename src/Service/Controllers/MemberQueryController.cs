namespace MemoryBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Business.Member.Providers;
    using Common.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Repository.Member.Models;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    public class MemberQueryController : ControllerBase
    {
        private readonly IMemberProvider memberProvider;

        public MemberQueryController(IMemberProvider memberProvider)
        {
            Contract.RequiresNotNull(memberProvider, nameof(memberProvider));

            this.memberProvider = memberProvider;
        }

        /// <summary>
        /// Gets all members.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <returns>Returns all members.</returns>
        [HttpPost("GetAllMembers")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<MemberReadModel>))]
        [SwaggerOperation(OperationId = "GetAllMembers")]
        public async Task<ActionResult> GetAllMembersAsync([FromQuery] Guid memoryBookUniverseId)
        {
            IList<MemberReadModel> members = await this.memberProvider.GetAllMembers(memoryBookUniverseId).ConfigureAwait(false);

            return this.Ok(members);
        }

        /// <summary>
        /// Gets members by id.
        /// </summary>
        /// <param name="memoryBookUniverseId">The universe id.</param>
        /// <param name="memberIds">The member ids.</param>
        /// <returns>Returns members.</returns>
        [HttpPost("GetMembersById")]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(IList<MemberReadModel>))]
        [SwaggerOperation(OperationId = "GetMembersById")]
        public async Task<ActionResult> GetMembersByIdAsync([FromQuery] Guid memoryBookUniverseId, [FromBody] IEnumerable<Guid> memberIds)
        {
            IList<MemberReadModel> members = await this.memberProvider.GetMembers(memoryBookUniverseId, memberIds?.ToList()).ConfigureAwait(false);

            return this.Ok(members);
        }
    }
}