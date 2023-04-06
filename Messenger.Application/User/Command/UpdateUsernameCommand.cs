using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;
using Messenger.Models.User.Request;

namespace Messenger.Application.Command
{
    public class UpdateUsernameCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateUsernameHandler : IRequestHandler<UpdateUsernameCommand, ResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;

        public UpdateUsernameHandler(IUserRepository userRepository, IResponseFactory responseFactory)
        {
            _userRepository = userRepository;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var updateRequest = new UpdateUserRequest { Id = request.Id, Name = request.Name };

                await _userRepository.UpdateAsync(updateRequest);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex) 
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
