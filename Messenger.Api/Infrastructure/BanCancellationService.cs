using MediatR;
using Messenger.Application.Command;

namespace Messenger.Api.Infrastructure
{
    public class BanCancellationService : BackgroundService
    {
        private const int Delay = 24 * 60 * 1000; //24 hours
        private readonly IServiceProvider _serviceProvider;

        public BanCancellationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                using(var scope = _serviceProvider.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<BanCancellationService>>();
                    var mediator = scope.ServiceProvider.GetService<IMediator>();
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var bans = await mediator.Send(new GetUserBanQuery { MaximalEnd = DateTime.Now });
                        foreach (var ban in bans)
                        {
                            Console.WriteLine(ban.Id);
                            await mediator.Send(new DeleteUserBanCommand { Id = ban.Id });
                            await mediator.Send(new DeleteUserClaimCommand { UserId = ban.UserId, Claim = "Ban" });
                        }
                        logger.LogWarning("Unbanned {num} users at {date}", bans.Count(), DateTime.Now);

                        await Task.Delay(Delay);
                    }
                }
            });
        }
    }
}
