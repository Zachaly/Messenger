using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.ChatMessage;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/chat-message")]
    [Authorize]
    public class ChatMessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatMessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns chat messages
        /// </summary>
        /// <response code="200">List of chat message models</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<ChatMessageModel>>> GetAsync([FromQuery] GetChatMessageQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Adds new chat message
        /// </summary>
        /// <response code="201">Chat message created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Unbanned]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddChatMessageCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("");
        }

        /// <summary>
        /// Returns number of chat messages
        /// </summary>
        /// <response code="200">Chat message count</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetChatMessageCountQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }
    }
}
