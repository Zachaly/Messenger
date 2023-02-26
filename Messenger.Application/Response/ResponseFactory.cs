using FluentValidation.Results;
using Messenger.Application.Abstraction;
using Messenger.Models.Response;

namespace Messenger.Application
{
    public class ResponseFactory : IResponseFactory
    {
        public ResponseModel CreateCreatedSuccess(long id)
            => new ResponseModel
            {
                NewEntityId = id,
                Success = true,
            };

        public ResponseModel CreateFailure(string errorMessage)
            => new ResponseModel 
            { 
                Success = false,
                Error = errorMessage
            };

        public DataResponseModel<T> CreateFailure<T>(string errorMessage)
            => new DataResponseModel<T> 
            {
                Success = false,
                Error = errorMessage,
                Data = default(T)
            };

        public ResponseModel CreateSuccess()
            => new ResponseModel
            {
                Success = true,
            };

        public DataResponseModel<T> CreateSuccess<T>(T data)
            => new DataResponseModel<T>
            {
                Data = data,
                Success = true
            };

        public ResponseModel CreateValidationError(ValidationResult result)
            => new ResponseModel
            {
                Success = false,
                Errors = result.ToDictionary()
            };
    }
}
