using MediatR;
using Messenger.Database.Repository;
using Messenger.Models.MessageReport;
using Messenger.Models.MessageReport.Request;

namespace Messenger.Application.Command
{
    public class GetMessageReportQuery : GetMessageReportRequest, IRequest<IEnumerable<MessageReportModel>>
    {
    }

    public class GetMessageReportHandler : IRequestHandler<GetMessageReportQuery, IEnumerable<MessageReportModel>>
    {
        private readonly IMessageReportRepository _messageReportRepository;

        public GetMessageReportHandler(IMessageReportRepository messageReportRepository)
        {
            _messageReportRepository = messageReportRepository;
        }

        public Task<IEnumerable<MessageReportModel>> Handle(GetMessageReportQuery request, CancellationToken cancellationToken)
        {
            return _messageReportRepository.GetAsync(request);
        }
    }
}
