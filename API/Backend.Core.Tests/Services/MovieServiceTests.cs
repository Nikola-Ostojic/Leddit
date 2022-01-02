using Backend.Core.Interfaces;
using Backend.Core.Services;
using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using Backend.DAL.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Core.Tests.Services
{
    public class MovieServiceTests
    {
        private readonly IMovieService _movieService;
        private readonly Mock<IMovieRepository> _movieRepositoryMock;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _movieService = new MovieService(_movieRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_MovieExists_ShouldReturnMovie()
        {
            // Arrange
            var id = 1;

            _movieRepositoryMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(() => null);

            // Act
            var result = await _movieService.Get(id);

            // Assert
            Assert.Null(result);
            _movieRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task Get_MovieDoesntExist_ShouldReturnNull()
        {
            // Arrange
            var id = 1;
            var movieEntity = new MovieEntity
            {
                Id = id,
                Name = "Mission impossible"
            };

            _movieRepositoryMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(() => movieEntity);

            // Act
            var result = await _movieService.Get(id);

            // Assert
            Assert.NotNull(result);
            _movieRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task GetMovies_ShouldReturnMovies()
        {
            // Arrange
            //string movieName, int page, int itemsPerPag
            var movieName = "Ava";
            var page = 0;
            var itemsPerPage = 20;

            _movieRepositoryMock
                .Setup(x => x.GetMovies(movieName, page, itemsPerPage))
                .ReturnsAsync(new Pageable<MovieEntity>
                {
                    Page = 1,
                    TotalItems = 2,
                    TotalPages = 1,
                    Data = new List<MovieEntity>
                    {
                        new MovieEntity
                        {
                            Id= 1,
                            Name = "Avatar",
                        },
                        new MovieEntity
                        {
                            Id= 2,
                            Name = "lAVA",
                        },
                    }
                });

            // Act
            var result = await _movieService.GetMovies(movieName, page, itemsPerPage);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(1, result.Page);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async Task Create_ShouldReturnTheCreatedMovie()
        {
            // Arrange
            var id = 1;
            var movieEntity = new MovieEntity
            {
                Id = id,
                Name = "Mission impossible"
            };

            _movieRepositoryMock
               .Setup(x => x.Create(movieEntity))
               .ReturnsAsync(() => movieEntity);

            // Act
            var result = await _movieService.Create(movieEntity);

            // Assert
            Assert.NotNull(result);
            _movieRepositoryMock.Verify(x => x.Create(movieEntity), Times.Once);
        }

        [Fact]
        public async Task Update_MovieExists_ShouldReturnTheUpdateMovie()
        {
            // Arrange
            var id = 1;
            var movieEntity = new MovieEntity
            {
                Id = id,
                Name = "Mission impossible"
            };

            _movieRepositoryMock
               .Setup(x => x.Update(movieEntity))
               .ReturnsAsync(() => movieEntity);

            // Act
            var result = await _movieService.Update(movieEntity);

            // Assert
            Assert.NotNull(result);
            _movieRepositoryMock.Verify(x => x.Update(movieEntity), Times.Once);
        }

        [Fact]
        public async Task Update_MovieDoesntExist_ShouldReturnNull()
        {
            // Arrange
            var id = 1;
            var movieEntity = new MovieEntity
            {
                Id = id,
                Name = "Mission impossible"
            };

            _movieRepositoryMock
               .Setup(x => x.Update(movieEntity))
               .ReturnsAsync(() => null);

            // Act
            var result = await _movieService.Update(movieEntity);

            // Assert
            Assert.Null(result);
            _movieRepositoryMock.Verify(x => x.Update(movieEntity), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            var id = 1;
            _movieRepositoryMock
                .Setup(x => x.Delete(id));

            // Act
            _movieService.Delete(id);

            // Assert
            _movieRepositoryMock.Verify(x => x.Delete(id), Times.Once);
        }
    }
}
