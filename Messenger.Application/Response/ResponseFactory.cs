using FluentValidation.Results;
using Messenger.Application.Abstraction;
using Messenger.Models.Response;

namespace Messenger.Application
{
    public class ResponseFactory : IResponseFactory
    {
        public ResponseModel CreateCreatedSuccess(long id)
        {
            throw new NotImplementedException();
        }

        public ResponseModel CreateFailure(string errorMessage)
        {
            throw new NotImplementedException();
        }

        public DataResponseModel<T> CreateFailure<T>(string errorMessage)
        {
            throw new NotImplementedException();
        }

        public ResponseModel CreateSuccess()
        {
            throw new NotImplementedException();
        }

        public DataResponseModel<T> CreateSuccess<T>(T data)
        {
            throw new NotImplementedException();
        }

        public ResponseModel CreateValidationError(ValidationResult result)
        {
            throw new NotImplementedException();
        }
    }
}
