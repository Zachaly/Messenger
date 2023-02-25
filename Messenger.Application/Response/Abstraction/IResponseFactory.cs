using Messenger.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Application.Abstraction
{
    public interface IResponseFactory
    {
        ResponseModel CreateSuccess();
        DataResponseModel<T> CreateSuccess<T>(T data);
        ResponseModel CreateFailure(string errorMessage);
        DataResponseModel<T> CreateFailure<T>(string errorMessage);
        ValidationResponse CreateValidationError(ValidationResult result);
    }
}
