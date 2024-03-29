﻿using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class DeleteUserBanCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteUserBanHandler : IRequestHandler<DeleteUserBanCommand, ResponseModel>
    {
        private readonly IUserBanRepository _userBanRepository;
        private readonly IResponseFactory _responseFactory;

        public DeleteUserBanHandler(IUserBanRepository userBanRepository, IResponseFactory responseFactory)
        {
            _userBanRepository = userBanRepository;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(DeleteUserBanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _userBanRepository.DeleteByIdAsync(request.Id);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
