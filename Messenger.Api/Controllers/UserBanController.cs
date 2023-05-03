using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.Response;
using Messenger.Models.UserBan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/user-ban")]
    [Authorize]
    public class UserBanController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserBanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of user bans
        /// </summary>
        /// <response code="200">List of bans</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserBanModel>>> GetAsync([FromQuery] GetUserBanQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Adds new user ban
        /// </summary>
        /// <response code="204">User banned successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "Moderator")]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddUserBanCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Removed user ban
        /// </summary>
        /// <response code="204">User unbanned successfully</response>
        /// <response code="400">Invalid data</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Policy = "Moderator")]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long id)
        {
            var res = await _mediator.Send(new DeleteUserBanCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
