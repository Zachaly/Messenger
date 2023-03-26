using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.Friend;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/friend")]
    [Authorize]
    public class FriendController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FriendController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of user's friends
        /// </summary>
        /// <response code="200">List of friends</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<FriendListItem>>> GetAsync([FromQuery] GetFriendsQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Removes a friend from database
        /// </summary>
        /// <response code="204">Friend deleted successfully</response>
        /// <response code="400">Failed to delete a friend</response>
        [HttpDelete("{user1Id}/{user2Id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long user1Id, long user2Id)
        {
            var res = await _mediator.Send(new DeleteFriendCommand { User1Id = user1Id, User2Id = user2Id });

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Returns number of user's friends
        /// </summary>
        /// <response code="200">Number of friends</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetFriendCountQuery query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
    }
}
