using MediatR;
using Messenger.Api.Hubs;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.DirectMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Api.Controllers
{
    [Authorize]
    [Route("/api/direct-message")]
    public class DirectMessageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<DirectMessageHub, IDirectMessageClient> _messageHub;

        public DirectMessageController(IMediator mediator, IHubContext<DirectMessageHub, IDirectMessageClient> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }

        /// <summary>
        /// Adds new direct message with data given in request
        /// </summary>
        /// <response code="204">Message added successfully</response>
        /// <response code="400">Failed to add the message</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Unbanned]
        public async Task<ActionResult> PostAsync(AddDirectMessageCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("");
        }

        /// <summary>
        /// Returns list of messages
        /// </summary>
        /// <response code="200">List of messages</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<DirectMessageModel>>> GetAsync([FromQuery] GetDirectMessagesQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Updates message with given data
        /// </summary>
        /// <response code="204">Message updated successfully</response>
        /// <response code="400">Failed to update the message</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Unbanned]
        public async Task<ActionResult> PutAsync(UpdateDirectMessageCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Returns count of direct messages
        /// </summary>
        /// <response code="200">Number of messages</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetDirectMessageCountQuery query)
        {
            var res = await _mediator.Send(query);
            
            return Ok(res);
        }
    }
}
