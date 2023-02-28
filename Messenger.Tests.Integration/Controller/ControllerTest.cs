using Dapper;
using Messenger.Models.User;
using Messenger.Models.User.Request;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Messenger.Tests.Integration.Controller
{
    
    public class ControllerTest : IDisposable
    {
        protected readonly HttpClient _httpClient;
        protected readonly WebApplicationFactory<Program> _webFactory;
        private readonly string _authUsername = "authorized";
        protected long _authorizedUserId = 0;
        private readonly string _connectionString = "Server=localhost;Database=MessengerTest;Trusted_Connection=True;";
        private readonly string[] _tearDownQueries = 
            { 
                "TRUNCATE TABLE [User]"
            };

        public ControllerTest()
        {
            _webFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["ConnectionString"] = _connectionString
                    });
                });
            });

            _httpClient = _webFactory.CreateClient();
        }

        public void Dispose()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                foreach(var query in _tearDownQueries)
                {
                    connection.Query(query);
                }
            }
        }

        protected async Task Authorize()
        {
            var registerRequest = new AddUserRequest { Login = _authUsername, Name = _authUsername, Password = "zaq1@WSX" };

            await _httpClient.PostAsJsonAsync("/api/user", registerRequest);

            var loginRequest = new LoginRequest { Login = _authUsername, Password = registerRequest.Password };

            var response = await _httpClient.PostAsJsonAsync("/api/user/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", content.AuthToken);
        }

        protected void ExecuteQuery(string query, object? param = null)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Query(query, param);
            }
        }

        protected IEnumerable<T> GetFromDatabase<T>(string query, object? param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<T>(query, param);
            }
        }
    }
}
