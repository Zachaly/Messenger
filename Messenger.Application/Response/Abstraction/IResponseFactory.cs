using FluentValidation.Results;
using Messenger.Models.Response;

namespace Messenger.Application.Abstraction
{
    public interface IResponseFactory
    {
        ResponseModel CreateSuccess();
        DataResponseModel<T> CreateSuccess<T>(T data);
        ResponseModel CreateFailure(string errorMessage);
        DataResponseModel<T> CreateFailure<T>(string errorMessage);
        ResponseModel CreateValidationError(ValidationResult result);
        ResponseModel CreateCreatedSuccess(long id);
    }
}
