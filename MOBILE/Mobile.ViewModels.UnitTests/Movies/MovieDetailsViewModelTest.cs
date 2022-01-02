using Mobile.Core.Dtos;
using Mobile.Core.Services.MoviesService;
using Mobile.ViewModels.Movies;
using Moq;
using NUnit.Framework;
using Splat;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.UnitTests.Movies
{
    [TestFixture]

    public class MovieDetailsViewModelTest : ViewModelTestBase
    {
        private MovieDetailsViewModel _target;
        private Mock<IMoviesService> _moviesService;

        [SetUp]
        public void SetUp()
        {
            _moviesService = new Mock<IMoviesService>() { DefaultValue = DefaultValue.Mock };
            _moviesService
               .Setup(x => x.Movie)
               .Returns(() => Observable.Return(new MovieDTO { Id = 1, Name = "1", ImageUrl = "1" }));
            _moviesService
               .Setup(x => x.GetMovie(It.IsAny<int>()))
               .Returns(() => Observable.Return(Unit.Default));

            Locator.CurrentMutable.Register(() => _moviesService.Object, typeof(IMoviesService));
        }

        [Test]
        public void CallGetMovie_OnActivation()
        {
            // Arrange
            _target = new MovieDetailsViewModel(1, _schedulerService, _viewStackService.Object, _moviesService.Object);

            // Act
            _target.Activator.Activate();

            // Assert
            _moviesService.Verify(x => x.GetMovie(1), Times.Once);
        }
    }
}
