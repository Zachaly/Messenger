using MediatR;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Models.Response;
using Messenger.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates new user with data given in request
        /// </summary>
        /// <response code="201">User successfully created</response>
        /// <response code="400">Failed to create user</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(RegisterCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("");
        }

        /// <summary>
        /// Validates user password and returns auth token
        /// </summary>
        /// <response code="200">Login response</response>
        /// <response code="400">Username or pasword is invalid</response>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnOkOrBadRequest();
        }

        /// <summary>
        /// Returns list of users
        /// </summary>
        /// <response code="200">List of users</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAsync([FromQuery] GetUsersQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }
    }
}
