using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Integration.Controller
{
    public class AuthorizationPolicyTests : ControllerTest
    {
        const string ApiUrl = "/api/test";

        private string AdminEndpoint { get => $"{ApiUrl}/admin"; }
        private string ModeratorEndpoint { get => $"{ApiUrl}/moderator"; }
        private string BanEndpoint { get => $"{ApiUrl}/ban"; }

        [Fact]
        public async Task AdminPolicy_Admin_Allowed()
        {
            await AuthorizeAdmin();

            var response = await _httpClient.GetAsync(AdminEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AdminPolicy_Moderator_NotAllowed()
        {
            await AuthorizeModerator();

            var response = await _httpClient.GetAsync(AdminEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task AdminPolicy_NoClaim_NotAllowed()
        {
            await Authorize();

            var response = await _httpClient.GetAsync(AdminEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task AdminPolicy_Banned_NotAllowed()
        {
            await AuthorizeBanned();

            var response = await _httpClient.GetAsync(AdminEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task ModeratorPolicy_Admin_Allowed()
        {
            await AuthorizeAdmin();

            var response = await _httpClient.GetAsync(ModeratorEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ModeratorPolicy_Moderator_Allowed()
        {
            await AuthorizeModerator();

            var response = await _httpClient.GetAsync(ModeratorEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ModeratorPolicy_NoClaim_NotAllowed()
        {
            await Authorize();

            var response = await _httpClient.GetAsync(ModeratorEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task ModeratorPolicy_Banned_Allowed()
        {
            await AuthorizeBanned();

            var response = await _httpClient.GetAsync(ModeratorEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UnbannedPolicy_Banned_NotAllowed()
        {
            await AuthorizeBanned();

            var response = await _httpClient.GetAsync(BanEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UnbannedPolicy_NoClaim_Allowed()
        {
            await Authorize();

            var response = await _httpClient.GetAsync(BanEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
