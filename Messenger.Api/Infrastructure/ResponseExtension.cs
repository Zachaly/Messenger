using Messenger.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Infrastructure
{
    public static class ResponseExtension
    {
        public static ActionResult ReturnCreatedOrBadRequest(this ResponseModel response, string location)
        {
            if (response.Success)
            {
                return new CreatedResult(location, response);
            }

            return new BadRequestObjectResult(response);
        }

        public static ActionResult<T> ReturnOkOrBadRequest<T>(this DataResponseModel<T> response)
        {
            if (response.Success)
            {
                return new OkObjectResult(response.Data);
            }

            return new BadRequestObjectResult(response);
        }

        public static ActionResult ReturnNoContentOrBadRequest(this ResponseModel response)
        {
            if(response.Success)
            {
                return new NoContentResult();
            }

            return new BadRequestObjectResult(response);
        }
    }
}
