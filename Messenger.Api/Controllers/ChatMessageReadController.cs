using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/chat-message-read")]
    [Authorize]
    public class ChatMessageReadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatMessageReadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Marks chat message as read by given user
        /// </summary>
        /// <response code="204">Read added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddChatMessageReadCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
