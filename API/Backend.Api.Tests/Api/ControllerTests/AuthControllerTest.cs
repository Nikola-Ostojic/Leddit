using Backend.Api.DTOs.Request;
using Backend.Api.DTOs.Response;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Backend.Api.Tests.Api.ControllerTests
{
    public class AuthControllerTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _server;
        public AuthControllerTest(ServerFixture server)
        {
            _server = server;
        }

        [Fact]
        public async void Login_InvalidCredentials_BadRequest()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var loginReq = new LoginRequestDTO
            {
                Email = "Blabla bla",
                Password = "Bla Bla"
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginReq), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("api/auth/login", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void Login_ValidCredentials_Ok()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var loginReq = new LoginRequestDTO
            {
                Email = "user@levi9.com",
                Password = "User"
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginReq), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("api/auth/login", content);

            // Assert
            var stream = await response.Content.ReadAsStreamAsync();
            TokenResponseDTO data = null;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                data = JsonConvert.DeserializeObject<TokenResponseDTO>(await reader.ReadToEndAsync());
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(data.AccessToken);
            Assert.NotNull(data.RefreshToken);
        }

        [Fact]
        public async void Register_WithEmailThatIsInUse_BadRequest()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var loginReq = new RegisterRequestDTO
            {
                ConfirmPassword = "User",
                Password = "User",
                Email = "user@levi9.com",
                UserName = "User"
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginReq), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("api/auth/register", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void Register_WithUniqueEmail_Ok()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var loginReq = new RegisterRequestDTO
            {
                ConfirmPassword = "User123",
                Password = "User123",
                Email = "user123@levi9.com",
                UserName = "User123"
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(loginReq), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/auth/register", content);
            var stream = await response.Content.ReadAsStreamAsync();
            TokenResponseDTO data = null;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                data = JsonConvert.DeserializeObject<TokenResponseDTO>(await reader.ReadToEndAsync());
            }

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(data.AccessToken);
            Assert.NotNull(data.RefreshToken);
        }
    }
}
