using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.MessageReport;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/message-report")]
    [Unbanned]
    public class MessageReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessageReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of message reports
        /// </summary>
        /// <response code="200">List of reports</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [Authorize(Policy = "Moderator")]
        public async Task<ActionResult<IEnumerable<MessageReportModel>>> GetAsync([FromQuery] GetMessageReportQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Adds new message report
        /// </summary>
        /// <response code="204">Report added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddMessageReportCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Updates message report
        /// </summary>
        /// <response code="204">Report updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync(UpdateMessageReportCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
