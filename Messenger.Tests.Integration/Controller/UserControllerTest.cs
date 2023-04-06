using Messenger.Application.Command;
using Messenger.Domain.Entity;
using Messenger.Models.User;
using Messenger.Models.User.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Integration.Controller
{
    public class UserControllerTest : ControllerTest
    {
        const string ApiUrl = "/api/user";

        public UserControllerTest() : base()
        {

        }

        [Fact]
        public async Task PostAsync_Success()
        {
            var request = new RegisterCommand
            {
                Login = "login",
                Name = "Test Name",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, request);
            
            var userCount = GetFromDatabase<User>("SELECT * FROM [User]").Count();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(1, userCount);
        }

        [Fact]
        public async Task PostAsync_LoginTaken_Fail()
        {
            var queryParam = new
            {
                Login = "login",
                Name = "name",
                PasswordHash = "hash"
            };

            InsertUser(queryParam);

            var request = new RegisterCommand
            {
                Login = "login",
                Name = "Test Name",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, request);

            var userCount = GetFromDatabase<User>("SELECT * FROM [User]").Count();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(1, userCount);
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            await Authorize();

            var users = new List<User>
            {
                new User { Login = "login1", Name = "name1", PasswordHash = "hash1" },
                new User { Login = "login2", Name = "name2", PasswordHash = "hash2" },
                new User { Login = "login3", Name = "name3", PasswordHash = "hash3" },
                new User { Login = "login4", Name = "name4", PasswordHash = "hash4" },
            };

            foreach(var user in users)
            {
                InsertUser(user);
            }

            var response = await _httpClient.GetAsync($"{ApiUrl}?PageIndex=2&PageSize=2");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Single(content);
        }

        [Fact]
        public async Task LoginAsync_Success()
        {
            var addUserRequest = new AddUserRequest { Login = "login", Name = "name", Password = "zaq1@WSX" };

            await _httpClient.PostAsJsonAsync(ApiUrl, addUserRequest);

            var loginRequest = new LoginCommand { Login = addUserRequest.Login, Password = addUserRequest.Password };

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(content.AuthToken);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            await Authorize();

            var response = await _httpClient.GetAsync($"{ApiUrl}/{_authorizedUserId}");
            var content = await response.Content.ReadFromJsonAsync<UserModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_authUsername, content.Name);
        }

        [Fact]
        public async Task GetCurrentUserAsync_Success()
        {
            await Authorize();

            var response = await _httpClient.GetAsync($"{ApiUrl}/current");
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_authUsername, content.UserName);
            Assert.Equal(_authorizedUserId, content.UserId);
            Assert.Equal(_httpClient.DefaultRequestHeaders.Authorization.Parameter, content.AuthToken);
        }

        [Fact]
        public async Task UpdateUserAsync_Success()
        {
            await Authorize();

            var command = new UpdateUsernameCommand { Id = _authorizedUserId, Name = "new name" };
            var response = await _httpClient.PatchAsJsonAsync(ApiUrl, command);
            
            var user = GetFromDatabase<User>("SELECT * FROM [User]").First();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(command.Name, user.Name);
        }
    }
}
