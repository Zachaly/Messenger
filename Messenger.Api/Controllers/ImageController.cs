using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/image")]
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Returns profile image of given user
        /// </summary>
        /// <response code="200">Profile image</response>
        [HttpGet("profile/{userId}")]
        public async Task<FileStreamResult> GetProfileImageAsync(long userId)
        {
            var res = await _mediator.Send(new GetProfileImageQuery { UserId = userId });

            return new FileStreamResult(res, "image/png");
        }

        /// <summary>
        /// Returns direct message image with given id
        /// </summary>
        /// <response code="200">Profile image</response>
        [HttpGet("direct-message/{id}")]
        public async Task<FileStreamResult> GetDirectMessageImageAsync(long id)
        {
            var res = await _mediator.Send(new GetDirectMessageImageQuery { ImageId = id });

            return new FileStreamResult(res, "image/png");
        }

        /// <summary>
        /// Updates profile image of given user
        /// </summary>
        /// <response code="204">Image updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut("profile")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateProfileImageAsync([FromForm] SaveProfileImageCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Adds images to direct message
        /// </summary>
        /// <response code="204">Images added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost("direct-message")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostDirectMessageImage([FromForm] SaveDirectMessageImagesCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
