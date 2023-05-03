using MediatR;
using Messenger.Application.Abstraction;
using Messenger.Database.Repository;
using Messenger.Models.MessageReport.Request;
using Messenger.Models.Response;

namespace Messenger.Application.Command
{
    public class AddMessageReportCommand : AddMessageReportRequest, IRequest<ResponseModel>
    {
    }

    public class AddMessageReportHandler : IRequestHandler<AddMessageReportCommand, ResponseModel>
    {
        private readonly IMessageReportRepository _messageReportRepository;
        private readonly IMessageReportFactory _messageReportFactory;
        private readonly IResponseFactory _responseFactory;

        public AddMessageReportHandler(IMessageReportRepository messageReportRepository, IMessageReportFactory messageReportFactory,
            IResponseFactory responseFactory)
        {
            _messageReportRepository = messageReportRepository;
            _messageReportFactory = messageReportFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(AddMessageReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var report = _messageReportFactory.Create(request);

                await _messageReportRepository.InsertAsync(report);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
