using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.ChatUser;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/chat-user")]
    [Authorize]
    public class ChatUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of chat users
        /// </summary>
        /// <response code="200">List of chat user models</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<ChatUserModel>>> GetAsync([FromQuery] GetChatUserQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Return number of chat users
        /// </summary>
        /// <response code="200">Number of chat users</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCountAsync([FromQuery] GetChatUserCountQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Adds new chat user
        /// </summary>
        /// <response code="204">User added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Unbanned]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddChatUserCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Updates specified chat user
        /// </summary>
        /// <response code="204">User updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PutAsync(UpdateChatUserCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Removes specified user from chat
        /// </summary>
        /// <response code="204">User removed successfully</response>
        /// <response code="400">Failed to remove user</response>
        [HttpDelete("{chatId}/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long chatId, long userId)
        {
            var res = await _mediator.Send(new DeleteChatUserCommand { ChatId = chatId, UserId = userId });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
