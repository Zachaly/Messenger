using FluentValidation;
using MediatR;
using Messenger.Application;
using Messenger.Application.Abstraction;
using Messenger.Models.Response;

namespace Messenger.Api.Pipeline
{
    public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, ResponseModel>
        where TRequest : IValidatedRequest
    {
        private readonly IValidator<TRequest> _validator;
        private readonly IResponseFactory _responseFactory;

        public ValidationPipeline(IValidator<TRequest> validator, IResponseFactory responseFactory)
        {
            _validator = validator;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> Handle(TRequest request, RequestHandlerDelegate<ResponseModel> next, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if (!validation.IsValid)
            {
                return Task.FromResult(_responseFactory.CreateValidationError(validation));
            }

            return next();
        }
    }
}
