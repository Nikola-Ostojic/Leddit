using Backend.Api.DTOs;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Backend.Api.Tests.Api.ControllerTests
{
    public class MoviesControllerTests : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _server;
        public MoviesControllerTests(ServerFixture server)
        {
            _server = server;
        }

        [Fact]
        public async void GetMovieById_WithoutAccessToken_Unauthorized()
        {
            var client = _server.Instance.CreateClient();

            var response = await client.GetAsync("api/movies/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void GetMovieById_WithToken_OK()
        {
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIyOWZjMDc2OC00NzkzLTRhZDctYWFmZi01MzQ3OTg3MWI2NTgiLCJpYXQiOiI5LzI1LzIwMjAgOTo1MTozNiIsInN1YiI6InVzZXJAbGV2aTkuY29tIiwidXNlcm5hbWUiOiJVc2VyIiwiZXhwIjoxNjA4ODA3MDk2LCJpc3MiOiJMZXZpOSBCYWNrZW5kIiwiYXVkIjoibGV2aTlVc2VycyIsInJvbGVzIjpbIlVzZXIiXX0.4X-q8ZOSGdAOH9LMZf-2iUDRiSPW5tfONnqNCPf9rFM";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("api/movies/1");
            var stream = await response.Content.ReadAsStreamAsync();
            MovieDTO data = null;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                data = JsonConvert.DeserializeObject<MovieDTO>(await reader.ReadToEndAsync());
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(data);
            Assert.Equal(1, data.Id);
        }

        [Fact]
        public async void GetMovies_WithoutAccessToken_Unauthorized()
        {
            var client = _server.Instance.CreateClient();

            var response = await client.GetAsync("api/movies");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void GetMovies_WithToken_OK()
        {
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIyOWZjMDc2OC00NzkzLTRhZDctYWFmZi01MzQ3OTg3MWI2NTgiLCJpYXQiOiI5LzI1LzIwMjAgOTo1MTozNiIsInN1YiI6InVzZXJAbGV2aTkuY29tIiwidXNlcm5hbWUiOiJVc2VyIiwiZXhwIjoxNjA4ODA3MDk2LCJpc3MiOiJMZXZpOSBCYWNrZW5kIiwiYXVkIjoibGV2aTlVc2VycyIsInJvbGVzIjpbIlVzZXIiXX0.4X-q8ZOSGdAOH9LMZf-2iUDRiSPW5tfONnqNCPf9rFM";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("api/movies");
            var stream = await response.Content.ReadAsStreamAsync();
            PageableDTO<MovieDTO> data = null;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                data = JsonConvert.DeserializeObject<PageableDTO<MovieDTO>>(await reader.ReadToEndAsync());
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(data);
            Assert.True(data.TotalItems >= 2);
            Assert.Equal(1, data.Page);
            Assert.Equal(1, data.TotalPages);
        }

        [Fact]
        public async void CreateMovie_WithoutToken_Unauthorized()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var movie = new MovieDTO
            {
                Name = "New Movie",
                ImageUrl = "New movie url"
            };
            var content = new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json");


            // Act
            var response = await client.PostAsync("api/movies", content);


            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void CreateMovie_WithTokenThatIsNotAdmin_Forbiden()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIyOWZjMDc2OC00NzkzLTRhZDctYWFmZi01MzQ3OTg3MWI2NTgiLCJpYXQiOiI5LzI1LzIwMjAgOTo1MTozNiIsInN1YiI6InVzZXJAbGV2aTkuY29tIiwidXNlcm5hbWUiOiJVc2VyIiwiZXhwIjoxNjA4ODA3MDk2LCJpc3MiOiJMZXZpOSBCYWNrZW5kIiwiYXVkIjoibGV2aTlVc2VycyIsInJvbGVzIjpbIlVzZXIiXX0.4X-q8ZOSGdAOH9LMZf-2iUDRiSPW5tfONnqNCPf9rFM";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var movie = new MovieDTO
            {
                Name = "New Movie",
                ImageUrl = "New movie url"
            };
            var content = new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json");


            // Act
            var response = await client.PostAsync("api/movies", content);


            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async void CreateMovie_WithTokenThatIsAdmin_Created()
        {
            // Arrange
            var client = _server.Instance.CreateClient();
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI4OGZjOGYyMC05ZDdkLTQ2NjUtODM3MC1mNzgyZGFkNzQ3ZDYiLCJpYXQiOiI5LzI1LzIwMjAgMTE6MTg6MDMiLCJzdWIiOiJhZG1pbkBsZXZpOS5jb20iLCJ1c2VybmFtZSI6IkFkbWluIiwiZXhwIjoxNjA4ODEyMjgzLCJpc3MiOiJMZXZpOSBCYWNrZW5kIiwiYXVkIjoibGV2aTlVc2VycyIsInJvbGVzIjpbIkFkbWluIl19.M9hwz43m5rLjNOJ3QZg4iAozOKByOkHfW7AbjxlDGCY";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var movie = new MovieDTO
            {
                Name = "New Movie",
                ImageUrl = "New movie url"
            };
            var content = new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json");


            // Act
            var response = await client.PostAsync("api/movies", content);


            // Assert
            var stream = await response.Content.ReadAsStreamAsync();
            MovieDTO data = null;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                data = JsonConvert.DeserializeObject<MovieDTO>(await reader.ReadToEndAsync());
            }

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(data);
        }
    }
}



