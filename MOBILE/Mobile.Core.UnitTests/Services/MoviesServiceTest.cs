using Mobile.Core.Api;
using Mobile.Core.Api.Rest;
using Mobile.Core.Dtos;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Services.MoviesService;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.Core.UnitTests.Services
{
    [TestFixture]
    public class MoviesServiceTest
    {
        private IMoviesService _target;
        private Mock<IApiService<IMoviesApi>> _moviesApiServiceMock;
        private Mock<IMoviesApi> _moviesApiMock;

        [SetUp]
        public void Setup()
        {
            _moviesApiMock = new Mock<IMoviesApi>();
            var moviesList = new List<MovieDTO>
            {
                {
                    new MovieDTO{ Id = 1,
                    Name= "One",
                    ImageUrl= "One"}
                },
                new MovieDTO
                {
                    Id = 2,
                    Name = "Two",
                    ImageUrl = "Two"
                }
            };

            _moviesApiMock
                .Setup(x => x.FetchMovies(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() => Observable.Return(new PageableDTO<MovieDTO>
                {
                    Data = moviesList,
                    Page = 1,
                    TotalItems = 2,
                    TotalPages = 1
                })
                );

            _moviesApiMock
               .Setup(x => x.FetchMovie(It.IsAny<int>()))
               .Returns(() => Observable.Return(moviesList.First()));

            _moviesApiMock
              .Setup(x => x.DeleteMovie(It.IsAny<int>()))
              .Returns(() => Observable.Return("Deleted"));

            _moviesApiMock
                .Setup(x => x.CreateMovie(It.IsAny<CreateMovieRequestDTO>()))
                 .Returns(() => Observable.Return(Unit.Default));

            _moviesApiServiceMock = new Mock<IApiService<IMoviesApi>>();
            _moviesApiServiceMock
                .Setup(x => x.GetClient())
                .Returns(() => _moviesApiMock.Object);
        }

        [Test]
        public void GetMovies_UpdatesTheMoviesCache()
        {
            // Arrange
            _target = new MoviesService(_moviesApiServiceMock.Object);


            // Act
            _target.GetMovies("test", 25).Subscribe();


            // Assert
            _moviesApiMock.Verify(x => x.FetchMovies("test", 25), Times.Once);
            Assert.AreEqual(2, _target.Movies.Count);
        }

        [Test]
        public void GetMovie_SetsTheMovie()
        {
            // Arrange
            _target = new MoviesService(_moviesApiServiceMock.Object);

            // Act
            _target.GetMovie(1).Subscribe();

            // Assert
            _moviesApiMock.Verify(x => x.FetchMovie(1), Times.Once);
            _target.Movie.Subscribe(m =>
            {
                Assert.AreEqual(1, m.Id);
                Assert.AreEqual("One", m.Name);
                Assert.AreEqual("One", m.ImageUrl);
            });
        }

        [Test]
        public void DeleteMovie_ShouldCallRestDeleteEndpoint()
        {
            // Arrange
            _target = new MoviesService(_moviesApiServiceMock.Object);

            // Act
            _target.Delete(1).Subscribe();

            // Assert
            _moviesApiMock.Verify(x => x.DeleteMovie(1), Times.Once);
        }

        [Test]
        public void AddMovie_ShouldCallRestAddEndpoint()
        {
            // Arrange
            _target = new MoviesService(_moviesApiServiceMock.Object);

            // Act
            _target.Create(new CreateMovieRequestDTO()).Subscribe();

            // Assert
            _moviesApiMock.Verify(x => x.CreateMovie(It.IsAny<CreateMovieRequestDTO>()), Times.Once);
        }
    }
}