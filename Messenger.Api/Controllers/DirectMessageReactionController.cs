using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Authorize]
    [Route("/api/direct-message-reaction")]
    public class DirectMessageReactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DirectMessageReactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Adds or updated direct message reaction and notifies message sender
        /// </summary>
        /// <response code="204">Reaction updated successfully</response>
        /// <response code="400">Invalid data</response>
        [HttpPut]
        public async Task<ActionResult<ResponseModel>> PutAsync(AddDirectMessageReactionCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Removes direct message reaction and notifies sender
        /// </summary>
        /// <response code="204">Reaction deleted successfully</response>
        /// <response code="400">Invalid data</response>
        [HttpDelete("{messageId}/{receiverId}")]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long messageId, long receiverId)
        {
            var res = await _mediator.Send(new DeleteDirectMessageReactionCommand { MessageId= messageId, ReceiverId = receiverId });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
