using MediatR;
using Messenger.Application.Command;
using Messenger.Models.Friend;
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
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<FriendListItem>>> GetAsync([FromQuery] GetFriendsQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }
    }
}
