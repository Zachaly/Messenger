using Messenger.Application.Abstraction;
using Messenger.Models.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Application
{
    public class ResponseFactory : IResponseFactory
    {
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

        public ValidationResponse CreateValidationError(ValidationResult result)
        {
            throw new NotImplementedException();
        }
    }
}
