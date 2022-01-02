using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using Backend.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.DAL.Tests.Repositories
{
    public class MovieRepositoryTests : RepositoryUnitTestsBase
    {
        private readonly IMovieRepository _movieRepository;

        public MovieRepositoryTests()
        {
            // Arrange
            _movieRepository = new MovieRepository(DbContext);
            DbContext.Movies.Add(new MovieEntity { Name = "Avatar" });
            DbContext.Movies.Add(new MovieEntity { Name = "Star Wars: The Rise Of Skywalker" });
            DbContext.SaveChanges();
        }

        [Fact]
        public async Task GetPeageable_ShouldReturnExpectedResults()
        {
            // Act
            var pageableResult = await _movieRepository.GetMovies("", 0, 0);

            // Assert
            Assert.Equal(2, pageableResult.Data.Count);
            Assert.Equal(2, pageableResult.TotalItems);
            Assert.Equal(1, pageableResult.TotalPages);

            var firstMovie = pageableResult.Data.First();
            Assert.Equal(1, firstMovie.Id);
            Assert.Equal("Avatar", firstMovie.Name);

            var secondMovie = pageableResult.Data.Last();
            Assert.Equal(2, secondMovie.Id);
            Assert.Equal("Star Wars: The Rise Of Skywalker", secondMovie.Name);
        }

        [Fact]
        public async Task GetMovieById_MovieExists_ReturnMovie()
        {
            var movie = await _movieRepository.Get(1);

            Assert.NotNull(movie);
            Assert.Equal(1, movie.Id);
            Assert.Equal("Avatar", movie.Name);
        }

        [Fact]
        public async Task GetMovieById_MovieDoesntExists_ReturnNull()
        {
            var movie = await _movieRepository.Get(3);

            Assert.Null(movie);
        }

        [Fact]
        public async Task CreateMovie_ShouldReturnTheCreatedMovie_AndIncreaseTheMoviesCount()
        {
            var newMovie = new MovieEntity { Name = "The Runaways" };
            var expectedNumberOfMovies = DbContext.Movies.Count() + 1;

            var createdMovie = await _movieRepository.Create(newMovie);

            Assert.NotNull(createdMovie);
            Assert.Equal(newMovie.Name, createdMovie.Name);
            Assert.True(newMovie.Id != 0);
            Assert.Equal(expectedNumberOfMovies, DbContext.Movies.Count());
        }

        [Fact]
        public async Task IfMovieExists_UpdateMovie_AndReturnUpdatedMovie()
        {
            var movie = DbContext.Movies.Find(1);
            movie.Name = "The Avengers";

            var updatedMovie = await _movieRepository.Update(movie);

            Assert.NotNull(updatedMovie);
            Assert.Equal(movie.Name, updatedMovie.Name);
            Assert.Equal(movie.Id, updatedMovie.Id);
        }

        [Fact]
        public async Task IfTheMovieToUpdate_DoesntExist_ReturnNull()
        {
            var movie = new MovieEntity { Id = 3, Name = "Jumanji" };

            var result = await _movieRepository.Update(movie);

            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_ShouldDeleteTheMovieFromTheDatabase()
        {
            var movieId = 1;

            await _movieRepository.Delete(movieId);
            Assert.Equal(1, DbContext.Movies.Count());
        }

        [Fact]
        public async Task Delete_AttemptToDeleteAnEntityThatDoesntExist_ShouldNotChangeTheTotalCount()
        {
            var movieId = 11;

            await _movieRepository.Delete(movieId);
            Assert.Equal(2, DbContext.Movies.Count());
        }
    }
}
