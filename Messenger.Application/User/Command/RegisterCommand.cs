using MediatR;
using Messenger.Models.Response;
using Messenger.Models.User.Request;

namespace Messenger.Application.Command
{
    public class RegisterCommand : AddUserRequest, IRequest<CreatedResponseModel> { }

    public class RegisterHandler : IRequestHandler<RegisterCommand, CreatedResponseModel>
    {
        public Task<CreatedResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
