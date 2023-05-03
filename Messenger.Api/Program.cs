using Messenger.Api.Hubs;
using Messenger.Api.Infrastructure;
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
builder.Services.AddHostedService<BanCancellationService>();

var app = builder.Build();

try
{
    using(var scope = app.Services.CreateScope())
    {
        await scope.CreateAdmin();
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
app.MapHub<ClaimHub>("ws/claim");

app.Run();

public partial class Program { }