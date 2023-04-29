using MediatR;
using Messenger.Api.Hubs;
using Messenger.Api.Infrastructure;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDatabase();
builder.Services.RegisterApplication();
builder.Services.ConfigureSwagger();

builder.ConfigureAuthorization();

builder.Services.AddSignalR();

var app = builder.Build();

try
{
    using(var scope = app.Services.CreateScope())
    {
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var admins = await mediator.Send(new GetUserClaimQuery { Value = "Admin" });

        if (!admins.Any())
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var res = await mediator.Send(new RegisterCommand 
            { 
                Login = config["DefaultAdminLogin"],
                Name = "admin",
                Password = config["DefaultAdminPassword"]
            });
            if(res.Success)
            {
                await mediator.Send(new AddUserClaimCommand { UserId = res.NewEntityId.GetValueOrDefault(), Value = "Admin" });
            }
        }
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
{
    opt.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000")
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<FriendHub>("/ws/friend");
app.MapHub<DirectMessageHub>("/ws/direct-message");
app.MapHub<StatusHub>("/ws/online-status");
app.MapHub<ChatHub>("ws/chat");

app.Run();

public partial class Program { }