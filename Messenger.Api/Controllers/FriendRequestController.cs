using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.Friend;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/friend-request")]
    [Authorize]
    public class FriendRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FriendRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns friend requests
        /// </summary>
        /// <response code="200">List of requests</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<FriendRequestModel>>> GetAsync([FromQuery] GetFriendRequestsQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Add new friend request and sends info to receiver
        /// </summary>
        /// <response code="201">Request created successfully</response>
        /// <response code="400">Request already exists</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddFriendCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("/api/friend-request");
        }

        /// <summary>
        /// Accepts or denies friend request and sends info to sender
        /// </summary>
        /// <response code="204"></response>
        [HttpPut("respond")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> RespondAsync(RespondToFriendRequestCommand command)
        {
            var res = await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Returns count of friend requests
        /// </summary>
        /// <response code="200">Count of friend requests</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetFriendRequestCountQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Deletes friend request with given id
        /// </summary>
        /// <response code="204">Request deleted successfully</response>
        /// <response code="400">Failed to delete request</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(int id)
        {
            var res = await _mediator.Send(new DeleteFriendRequestCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
