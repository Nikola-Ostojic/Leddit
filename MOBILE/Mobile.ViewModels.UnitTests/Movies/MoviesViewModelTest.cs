using DynamicData;
using Mobile.Core.Dtos;
using Mobile.Core.Runtime;
using Mobile.Core.Services.MoviesService;
using Mobile.ViewModels.Movies;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.UnitTests.Movies
{
    [TestFixture]
    public class MoviesViewModelTest : ViewModelTestBase
    {
        private MoviesViewModel _target;
        private Mock<IMoviesService> _moviesServiceMock;

        [SetUp]
        public void SetUp()
        {
            _moviesServiceMock = new Mock<IMoviesService>() { DefaultValue = DefaultValue.Mock };

            _moviesServiceMock
                .Setup(x => x.Movies)
                .Returns(new SourceCache<MovieDTO, int>(x => x.Id));
            _moviesServiceMock
               .Setup(x => x.Movie)
               .Returns(() => Observable.Return(new MovieDTO { Id = 1, Name = "1", ImageUrl = "1" }));
            _moviesServiceMock
                .Setup(x => x.GetMovies(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() => Observable.Return(Unit.Default));
            _moviesServiceMock
               .Setup(x => x.GetMovie(It.IsAny<int>()))
               .Returns(() => Observable.Return(Unit.Default));

            Locator.CurrentMutable.Register(() => _moviesServiceMock.Object, typeof(IMoviesService));

        }

        [Test]
        public void ShouldCallPushPage_ToMovieDetails_WhenMovieIsSelected()
        {
            // Arrange
            _target = new MoviesViewModel(_schedulerService, _viewStackService.Object, _moviesServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.SelectedMovie = new MovieCellViewModel(1, "1", "1");

            _schedulerService.AdvanceBy(TimeSpan.FromSeconds(3));

            // Assert
            _viewStackService.Verify(x => x.PushPage(
                 It.IsAny<MovieDetailsViewModel>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<bool>()));
        }

        [Test]
        public void ShouldCallPushPage_ToAddMovie_WhenAddIsClicked()
        {
            // Arrange
            _target = new MoviesViewModel(_schedulerService, _viewStackService.Object, _moviesServiceMock.Object);

            // Act
            _schedulerService.AdvanceBy(TimeSpan.FromSeconds(3));

            Observable.Return(Unit.Default).InvokeCommand(_target.ClickAddMovie);

            // Assert
            _viewStackService.Verify(x => x.PushPage(
                 It.IsAny<AddMovieViewModel>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<bool>()),
                Times.Exactly(1));
        }

        [Test]
        public void AddButtonEnabled_ShouldBeTrue_IfTheCurrentUserInAdminRole()
        {
            // Arrange
            _runtimeContextMock.Setup((x) => x.Role).Returns(() => "Admin");

            // Act
            _target = new MoviesViewModel(_schedulerService, _viewStackService.Object, _moviesServiceMock.Object, _runtimeContextMock.Object);

            // Assert
            Assert.True(_target.ClickAddButtonVisible);
        }

        [Test]
        public void AddButtonEnabled_ShouldBeTrue_IfTheCurrentUserIsNotInAdminRole()
        {
            // Arrange
            _runtimeContextMock.Setup((x) => x.Role).Returns(() => "User");

            // Act
            _target = new MoviesViewModel(_schedulerService, _viewStackService.Object, _moviesServiceMock.Object, _runtimeContextMock.Object);

            // Assert
            Assert.False(_target.ClickAddButtonVisible);
        }
    }
}
