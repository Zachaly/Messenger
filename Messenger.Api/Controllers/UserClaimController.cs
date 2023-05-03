using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.Response;
using Messenger.Models.UserClaim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/user-claim")]
    [Authorize(Policy = "Moderator")]
    public class UserClaimController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserClaimController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns user claims
        /// </summary>
        /// <response code="200">User claim list</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserClaimModel>>> GetAsync([FromQuery] GetUserClaimQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Creates new user claim with data given in request
        /// </summary>
        /// <response code="204">Claim added successfully</response>
        /// <response code="400">Invalid data</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddUserClaimCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Removes claim from given user
        /// </summary>
        /// <response code="204">Claim removed successfully</response>
        /// <response code="400">Invalid data</response>
        [HttpDelete("{userId}/{claim}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long userId, string claim)
        {
            var res = await _mediator.Send(new DeleteUserClaimCommand { Claim = claim, UserId = userId });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
