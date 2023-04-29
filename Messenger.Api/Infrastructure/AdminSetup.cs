using MediatR;
using Messenger.Application.Command;

namespace Messenger.Api.Infrastructure
{
    public static class AdminSetup
    {
        public static async Task CreateAdmin(this IServiceScope scope)
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var admins = await mediator.Send(new GetUserClaimQuery { Value = "Admin" });

            if (!admins.Any())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var res = await mediator.Send(new RegisterCommand
                {
                    Login = config["DefaultAdminLogin"],
                    Name = config["DefaultAdminName"],
                    Password = config["DefaultAdminPassword"]
                });
                if (res.Success)
                {
                    await mediator.Send(new AddUserClaimCommand { UserId = res.NewEntityId.GetValueOrDefault(), Value = "Admin" });
                    Console.WriteLine("Admin user added");
                }
            }
        }
    }
}
