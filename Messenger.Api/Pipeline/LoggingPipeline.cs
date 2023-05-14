using MediatR;

namespace Messenger.Api.Pipeline
{
    public class LoggingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingPipeline(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var warning = request.GetType().Name.Contains("Command");
            if (_httpContextAccessor.HttpContext is not null)
            {
                LogWithIP(request, warning);
            }
            else
            {
                LogWithoutIP(request, warning);
            }

            return next();
        }

        private void LogWithIP(TRequest request, bool warning)
        {
            if (warning)
            {
                _logger.LogWarning("{Req}: {Time}: {IP}",
                        request.GetType().Name, DateTime.Now, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            } else
            {
                _logger.LogInformation("{Req}: {Time}: {IP}",
                       request.GetType().Name, DateTime.Now, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            }
        }

        private void LogWithoutIP(TRequest request, bool warning)
        {
            if (warning)
            {
                _logger.LogWarning("{Req}: {Time}", request.GetType().Name, DateTime.Now);
            }
            else
            {
                _logger.LogInformation("{Req}: {Time}", request.GetType().Name, DateTime.Now);
            }
        }
    }
}
