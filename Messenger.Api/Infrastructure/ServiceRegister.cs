using Messenger.Application;
using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Connection;
using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Messenger.Database.Migrations;
using FluentMigrator.Runner;

namespace Messenger.Api.Infrastructure
{
    public static class ServiceRegister
    {
        public static void RegisterDatabase(this IServiceCollection collection)
        {
            collection.AddScoped<IConnectionFactory, ConnectionFactory>();
            collection.AddTransient<ISqlQueryBuilder, SqlQueryBuilder>();
            collection.AddScoped<IUserRepository, UserRepository>();
            collection.AddScoped<IFriendRepository, FriendRepository>();
            collection.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
            collection.AddScoped<IDirectMessageRepository, DirectMessageRepository>();
            collection.AddScoped<IDirectMessageImageRepository, DirectMessageImageRepository>();
            collection.AddScoped<IDirectMessageReactionRepository, DirectMessageReactionRepository>();
            collection.AddScoped<IChatRepository, ChatRepository>();
            collection.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            collection.AddScoped<IChatMessageReadRepository, ChatMessageReadRepository>();
            collection.AddScoped<IChatUserRepository, ChatUserRepository>();
            collection.AddScoped<IChatMessageImageRepository, ChatMessageImageRepository>();
            collection.AddScoped<IChatMessageReactionRepository, ChatMessageReactionRepository>();
            collection.AddScoped<IUserClaimRepository, UserClaimRepository>();
            collection.AddScoped<IUserBanRepository, UserBanRepository>();
            collection.AddScoped<IMessageReportRepository, MessageReportRepository>();
            collection.AddScoped<IMigrationManager, MigrationManager>();
        }

        public static void RegisterApplication(this IServiceCollection collection)
        {
            collection.AddScoped<IAuthService, AuthService>();
            collection.AddScoped<IResponseFactory, ResponseFactory>();
            collection.AddScoped<IUserFactory, UserFactory>();
            collection.AddScoped<IFriendFactory, FriendFactory>();
            collection.AddScoped<IDirectMessageFactory, DirectMessageFactory>();
            collection.AddScoped<INotificationService, NotificationService>();
            collection.AddScoped<IFileService, FileService>();
            collection.AddScoped<IFileFactory, FileFactory>();
            collection.AddScoped<IReactionFactory, ReactionFactory>();
            collection.AddScoped<IChatFactory, ChatFactory>();
            collection.AddScoped<IChatMessageFactory, ChatMessageFactory>();
            collection.AddScoped<IChatMessageReadFactory, ChatMessageReadFactory>();
            collection.AddScoped<IChatUserFactory, ChatUserFactory>();
            collection.AddScoped<IUserClaimFactory, UserClaimFactory>();
            collection.AddScoped<IMessageReportFactory, MessageReportFactory>();
            collection.AddScoped<IUserBanFactory, UserBanFactory>();
            collection.AddHttpContextAccessor();

            collection.AddMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblyContaining<LoginCommand>();
            });
        }

        public static void ConfigureAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"]);
                var key = new SymmetricSecurityKey(bytes);

                config.SaveToken = true;
                config.RequireHttpsMetadata = false;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidIssuer = builder.Configuration["Auth:Issuer"],
                    ValidAudience = builder.Configuration["Auth:Audience"],
                };
            })
            .AddJwtBearer("Websocket", config =>
            {
                var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"]);
                var key = new SymmetricSecurityKey(bytes);

                config.SaveToken = true;
                config.RequireHttpsMetadata = false;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidIssuer = builder.Configuration["Auth:Issuer"],
                    ValidAudience = builder.Configuration["Auth:Audience"],
                };
                config.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (context) =>
                    {
                        var path = context.HttpContext.Request.Path;
                        if (path.StartsWithSegments("/ws"))
                        {
                            var token = context.Request.Query["access_token"];

                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                context.Token = token;
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
                options.AddPolicy("Moderator", policy => policy.RequireClaim("Role", "Admin", "Moderator"));
                options.AddPolicy("Unbanned", policy => policy.Requirements.Add(new BanRequirement()));
            });

            builder.Services.AddSingleton<IAuthorizationHandler, BanAuthorizationHandler>();
        }

        public static void ConfigureSwagger(this IServiceCollection collection)
        {
            collection.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Messenger",
                    Description = ""
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var migrationManager = scope.ServiceProvider.GetRequiredService<IMigrationManager>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<MigrationManager>>();

                try
                {
                    migrationManager.CreateDatabase();
                    logger.LogInformation("Database created");
                }
                catch
                {
                    logger.LogCritical("Failed to create database");
                    return app;
                }

                try
                {
                    migrationManager.MigrateDatabase();
                    logger.LogInformation("Database migrated");
                }
                catch
                {
                    logger.LogCritical("Failed to migrate database");
                }

                return app;
            }
        }
    }
}
