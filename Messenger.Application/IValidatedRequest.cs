using MediatR;
using Messenger.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Application
{
    public interface IValidatedRequest : IRequest<ResponseModel>
    {
    }
}
