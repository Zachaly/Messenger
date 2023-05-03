using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/chat-message-reaction")]
    [Unbanned]
    public class ChatReactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatReactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Adds or replaces reaction for message
        /// </summary>
        /// <response code="204">Reaction added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddChatMessageReactionCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes chat message reaction
        /// </summary>
        /// <response code="204">Reaction deleted successfully</response>
        /// <response code="400">Failed to delete reaction</response>
        [HttpDelete("{userId}/{messageId}/{chatId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long userId, long messageId, long chatId)
        {
            var res = await _mediator.Send(new DeleteChatMessageReactionCommand { ChatId= chatId, MessageId = messageId, UserId = userId });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
