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
        }

        public static void RegisterApplication(this IServiceCollection collection)
        {
            collection.AddScoped<IAuthService, AuthService>();
            collection.AddScoped<IResponseFactory, ResponseFactory>();
            collection.AddScoped<IUserFactory, UserFactory>();
            collection.AddScoped<IFriendFactory, FriendFactory>();
            collection.AddScoped<IDirectMessageFactory, DirectMessageFactory>();
            collection.AddScoped<INotificationService, NotificationService>();

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
    }
}
