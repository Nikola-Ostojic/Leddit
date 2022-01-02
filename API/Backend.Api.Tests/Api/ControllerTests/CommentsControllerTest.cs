using Backend.Api.DTOs;
using Backend.Api.DTOs.Request;
using Backend.Api.DTOs.Response;
using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Backend.Api.Tests.Api.ControllerTests
{
    public class CommentsControllerTests : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _server;
        public CommentsControllerTests(ServerFixture server)
        {
            _server = server;
        }

        [Fact]
        public async void GetCommentById_OK()
        {
            var client = _server.Instance.CreateClient();

            var response = await client.GetAsync("api/comments/1");
            var stream = await response.Content.ReadAsStreamAsync();
            CommentResponseDTO data = null;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                data = JsonConvert.DeserializeObject<CommentResponseDTO>(await reader.ReadToEndAsync());
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(data);
            Assert.Equal(1, data.Id);
        }

        [Fact]
        public async void GetComments_OK()
        {
            var client = _server.Instance.CreateClient();

            var response = await client.GetAsync("api/comments?threadId=1");
            var stream = await response.Content.ReadAsStreamAsync();
            PageableDTO<CommentResponseDTO> data = null;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                data = JsonConvert.DeserializeObject<PageableDTO<CommentResponseDTO>>(await reader.ReadToEndAsync());
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(data);
            Assert.True(data.TotalItems >= 0);
            Assert.Equal(1, data.Page);
            Assert.Equal(1, data.TotalPages);
        }

        [Fact]
        public async void CreateComment_WithoutToken_Unauthorized()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var comment = new CommentRequestDTO
            {
                Content = "Beautiful day.",
                ThreadId = 1
            };
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");


            // Act
            var response = await client.PostAsync("api/comments", content);


            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void CreateComment_WithTokenThatIsAuthorized_Created()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI4OGZjOGYyMC05ZDdkLTQ2NjUtODM3MC1mNzgyZGFkNzQ3ZDYiLCJpYXQiOiI5LzI1LzIwMjAgMTE6MTg6MDMiLCJzdWIiOiJhZG1pbkBsZXZpOS5jb20iLCJ1c2VybmFtZSI6IkFkbWluIiwiZXhwIjoxNjA4ODEyMjgzLCJpc3MiOiJMZXZpOSBCYWNrZW5kIiwiYXVkIjoibGV2aTlVc2VycyIsInJvbGVzIjpbIkFkbWluIl19.M9hwz43m5rLjNOJ3QZg4iAozOKByOkHfW7AbjxlDGCY";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var comment = new CommentRequestDTO
            {
                Content = "Another beautiful day.",
                ThreadId = 1,

            };
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");


            // Act
            var response = await client.PostAsync("api/comments", content);


            // Assert
            var stream = await response.Content.ReadAsStreamAsync();
            CommentResponseDTO data = null;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                data = JsonConvert.DeserializeObject<CommentResponseDTO>(await reader.ReadToEndAsync());
            }

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(data);
        }

        [Fact]
        public async void CreateComment_WithTokenThatIsAuthorized_WithUnexistingThread_BadRequest()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI4OGZjOGYyMC05ZDdkLTQ2NjUtODM3MC1mNzgyZGFkNzQ3ZDYiLCJpYXQiOiI5LzI1LzIwMjAgMTE6MTg6MDMiLCJzdWIiOiJhZG1pbkBsZXZpOS5jb20iLCJ1c2VybmFtZSI6IkFkbWluIiwiZXhwIjoxNjA4ODEyMjgzLCJpc3MiOiJMZXZpOSBCYWNrZW5kIiwiYXVkIjoibGV2aTlVc2VycyIsInJvbGVzIjpbIkFkbWluIl19.M9hwz43m5rLjNOJ3QZg4iAozOKByOkHfW7AbjxlDGCY";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var comment = new CommentRequestDTO
            {
                Content = "Another beautiful day.",
                ThreadId = 55,

            };
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");


            // Act
            var response = await client.PostAsync("api/comments", content);


            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void UpdateComment_WithoutToken_Unauthorized()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var comment = new CommentRequestDTO
            {
                Id = 1,
                Content = "Another another beautiful day.",
                ThreadId = 1
            };
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");


            // Act
            var response = await client.PutAsync("api/comments/1", content);


            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void UpdateComment_WithTokenThatIsAuthorized_OK()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIyOWZjMDc2OC00NzkzLTRhZDctYWFmZi01MzQ3OTg3MWI2NTgiLCJpYXQiOiI5LzI1LzIwMjAgOTo1MTozNiIsInN1YiI6InVzZXJAbGV2aTkuY29tIiwidXNlcm5hbWUiOiJVc2VyIiwiZXhwIjoxNjA4ODA3MDk2LCJpc3MiOiJMZXZpOSBCYWNrZW5kIiwiYXVkIjoibGV2aTlVc2VycyIsInJvbGVzIjpbIlVzZXIiXX0.4X-q8ZOSGdAOH9LMZf-2iUDRiSPW5tfONnqNCPf9rFM";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var comment = new CommentRequestDTO
            {
                Id = 2,
                Content = "Sunny",
                ThreadId = 1
            };
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");


            // Act
            var response = await client.PutAsync("api/comments/2", content);


            // Assert
            var stream = await response.Content.ReadAsStreamAsync();
            CommentResponseDTO data = null;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                data = JsonConvert.DeserializeObject<CommentResponseDTO>(await reader.ReadToEndAsync());
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(data);
        }

        [Fact]
        public async void UpdateComment_WithTokenThatIsAuthorized_WithUnexistingThread_BadRequest()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI4OGZjOGYyMC05ZDdkLTQ2NjUtODM3MC1mNzgyZGFkNzQ3ZDYiLCJpYXQiOiI5LzI1LzIwMjAgMTE6MTg6MDMiLCJzdWIiOiJhZG1pbkBsZXZpOS5jb20iLCJ1c2VybmFtZSI6IkFkbWluIiwiZXhwIjoxNjA4ODEyMjgzLCJpc3MiOiJMZXZpOSBCYWNrZW5kIiwiYXVkIjoibGV2aTlVc2VycyIsInJvbGVzIjpbIkFkbWluIl19.M9hwz43m5rLjNOJ3QZg4iAozOKByOkHfW7AbjxlDGCY";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var comment = new CommentRequestDTO
            {
                Id = 1,
                Content = "Another beautiful day.",
                ThreadId = 55,

            };
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");


            // Act
            var response = await client.PutAsync("api/comments/55", content);


            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void UpdateComment_WithTokenThatIsAuthorized_DifferentUser_Forbidden()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIwOTg1M2QxYy1lMzE4LTRjZTgtODA4My01ODUyMTM4OTUxYWQiLCJpYXQiOiIxMC83LzIwMjAgMzo0Njo1MSBQTSIsInN1YiI6InVzZXIxQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiVXNlcjEiLCJleHAiOjE2MDk4NjUyMTEsImlzcyI6Ikxldmk5IEJhY2tlbmQiLCJhdWQiOiJsZXZpOVVzZXJzIiwicm9sZXMiOlsiVXNlciJdfQ.vMg4G7sYIq1jgYUYc9ekWhDfJxEX2XlALCHcLLvGwJA";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var comment = new CommentRequestDTO
            {
                Id = 1,
                Content = "Sunny",
                ThreadId = 1
            };
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync("api/comments/4", content);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async void DeleteComment_WithoutToken_Unauthorized()
        {
            // Arrange
            var client = _server.Instance.CreateClient();

            // Act
            var response = await client.DeleteAsync("api/comments/3");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void DeleteComment_WithTokenThatIsAuthorized_NoContent()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI4OGZjOGYyMC05ZDdkLTQ2NjUtODM3MC1mNzgyZGFkNzQ3ZDYiLCJpYXQiOiI5LzI1LzIwMjAgMTE6MTg6MDMiLCJzdWIiOiJhZG1pbkBsZXZpOS5jb20iLCJ1c2VybmFtZSI6IkFkbWluIiwiZXhwIjoxNjA4ODEyMjgzLCJpc3MiOiJMZXZpOSBCYWNrZW5kIiwiYXVkIjoibGV2aTlVc2VycyIsInJvbGVzIjpbIkFkbWluIl19.M9hwz43m5rLjNOJ3QZg4iAozOKByOkHfW7AbjxlDGCY";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await client.DeleteAsync("api/comments/3");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void DeleteComment_WithTokenThatIsAuthorized_DifferentUser_Forbidden()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIwOTg1M2QxYy1lMzE4LTRjZTgtODA4My01ODUyMTM4OTUxYWQiLCJpYXQiOiIxMC83LzIwMjAgMzo0Njo1MSBQTSIsInN1YiI6InVzZXIxQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiVXNlcjEiLCJleHAiOjE2MDk4NjUyMTEsImlzcyI6Ikxldmk5IEJhY2tlbmQiLCJhdWQiOiJsZXZpOVVzZXJzIiwicm9sZXMiOlsiVXNlciJdfQ.vMg4G7sYIq1jgYUYc9ekWhDfJxEX2XlALCHcLLvGwJA";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await client.DeleteAsync("api/comments/2");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async void DeleteComment_WithTokenThatIsAuthorized_Admin_NoContent()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI0NmZjYmYzMi03ZjljLTQwODUtYTM1NC1mNWM5MWQ3NDdlM2YiLCJpYXQiOiIxMC83LzIwMjAgMzo1MzozOSBQTSIsInN1YiI6ImFkbWluQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiQWRtaW4iLCJleHAiOjE2MDk2NDk2MTksImlzcyI6Ikxldmk5IEJhY2tlbmQiLCJhdWQiOiJsZXZpOVVzZXJzIiwicm9sZXMiOlsiQWRtaW4iXX0.PEwTtquuArmWVhNfpWCUOkx4xKxwPycEB5bgWvgr7Kg";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await client.DeleteAsync("api/comments/4");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void DeleteComment_WithTokenThatIsAuthorized_Admin_NotExistentThread()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI0NmZjYmYzMi03ZjljLTQwODUtYTM1NC1mNWM5MWQ3NDdlM2YiLCJpYXQiOiIxMC83LzIwMjAgMzo1MzozOSBQTSIsInN1YiI6ImFkbWluQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiQWRtaW4iLCJleHAiOjE2MDk2NDk2MTksImlzcyI6Ikxldmk5IEJhY2tlbmQiLCJhdWQiOiJsZXZpOVVzZXJzIiwicm9sZXMiOlsiQWRtaW4iXX0.PEwTtquuArmWVhNfpWCUOkx4xKxwPycEB5bgWvgr7Kg";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await client.DeleteAsync("api/comments/25");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
