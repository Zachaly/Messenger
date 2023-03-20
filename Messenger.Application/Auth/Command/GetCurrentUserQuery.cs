using MediatR;
using Messenger.Models.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Application.Auth.Command
{
    public class GetCurrentUserQuery : IRequest<LoginResponse>
    {

    }

    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, LoginResponse>
    {
        public GetCurrentUserHandler(IHttpContextAccessor)
        {

        }

        public Task<LoginResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
