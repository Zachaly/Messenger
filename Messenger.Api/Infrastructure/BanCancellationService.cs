using MediatR;
using Messenger.Application.Command;

namespace Messenger.Api.Infrastructure
{
    public class BanCancellationService : BackgroundService
    {
        private const int Delay = 24 * 60 * 1000; //24 hours
        private readonly IMediator _mediator;

        public BanCancellationService(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                while(!stoppingToken.IsCancellationRequested)
                {
                    var bans = await _mediator.Send(new GetUserBanQuery { MaximalEnd = DateTime.Now });
                    foreach (var ban in bans) 
                    {
                        await _mediator.Send(new DeleteUserBanCommand { Id = ban.Id });
                        await _mediator.Send(new DeleteUserClaimCommand { UserId = ban.UserId, Claim = "Ban" });
                    }
                    Console.WriteLine($"Unbanned {bans.Count()} users");

                    await Task.Delay(Delay);
                }
            });
        }
    }
}
