﻿using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.MessageReport.Request;
using Messenger.Models.Response;

namespace Messenger.Application.MessageReport.Command
{
    public class UpdateMessageReportCommand : UpdateMessageReportRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateMessageReportHandler : IRequestHandler<UpdateMessageReportCommand, ResponseModel>
    {
        private readonly IMessageReportRepository _messageReportRepository;
        private readonly IResponseFactory _responseFactory;

        public UpdateMessageReportHandler(IMessageReportRepository messageReportRepository, IResponseFactory responseFactory)
        {
            _messageReportRepository = messageReportRepository;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> Handle(UpdateMessageReportCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
