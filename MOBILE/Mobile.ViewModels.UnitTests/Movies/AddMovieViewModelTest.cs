using Mobile.Core.Dtos;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Services.MoviesService;
using Mobile.ViewModels.Movies;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.UnitTests.Movies
{
    [TestFixture]
    public class AddMovieViewModelTest : ViewModelTestBase
    {
        private AddMovieViewModel _target;
        private Mock<IMoviesService> _moviesServiceMock;

        [SetUp]
        public void SetUp()
        {
            _moviesServiceMock = new Mock<IMoviesService>() { DefaultValue = DefaultValue.Mock };

            _moviesServiceMock
              .Setup(x => x.Movie)
              .Returns(() => Observable.Return(new MovieDTO { Id = 1, Name = "1", ImageUrl = "1" }));
            _moviesServiceMock
               .Setup(x => x.GetMovie(It.IsAny<int>()))
               .Returns(() => Observable.Return(Unit.Default));
            _moviesServiceMock
               .Setup(x => x.Create(It.IsAny<CreateMovieRequestDTO>()))
               .Returns(() => Observable.Return(Unit.Default));
        }

        [Test]
        public void AddCommand_ShouldBeDisabled_IfNameIsEmpty()
        {
            // Arrange
            _target = new AddMovieViewModel(_schedulerService, _viewStackService.Object, _moviesServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.Name = "";
            _target.ImageUrl = "ImageUrl";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.AddMovie.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }

        [Test]
        public void AddCommand_ShouldBeDisabled_IfImageUrlIsEmpty()
        {
            // Arrange
            _target = new AddMovieViewModel(_schedulerService, _viewStackService.Object, _moviesServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.Name = "Name";
            _target.ImageUrl = "";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.AddMovie.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }

        [Test]
        public void AddCommand_ShouldBeEnabled_IfNameAndImageUrlAreNotEmpty()
        {
            // Arrange
            _target = new AddMovieViewModel(_schedulerService, _viewStackService.Object, _moviesServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.Name = "Name";
            _target.ImageUrl = "ImageUrl";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.AddMovie.CanExecute.Subscribe(canExecute => Assert.IsTrue(canExecute));
        }

        [Test]
        public void WhenExecuted_CheckIfPopPageIsCalled()
        {
            // Arrange
            _target = new AddMovieViewModel(_schedulerService, _viewStackService.Object, _moviesServiceMock.Object);
            _target.Name = "Name";
            _target.ImageUrl = "ImageUrl";

            // Act
            Observable.Return(Unit.Default).InvokeCommand(_target.AddMovie);

            // Assert
            _moviesServiceMock
                .Verify(x => x.Create(
                    It.IsAny<CreateMovieRequestDTO>()),
                    Times.Once);
        }
    }
}
