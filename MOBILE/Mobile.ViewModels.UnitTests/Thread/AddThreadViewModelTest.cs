using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using Mobile.Core.Services.ThreadService;
using Mobile.ViewModels.Thread;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.UnitTests.Thread
{
    [TestFixture]
    public class AddThreadViewModelTest : ViewModelTestBase
    {
        private AddThreadViewModel _target;
        private Mock<IThreadService> _threadsServiceMock;

        [SetUp]
        public void SetUp()
        {
            _threadsServiceMock = new Mock<IThreadService>() { DefaultValue = DefaultValue.Mock };

            _threadsServiceMock
              .Setup(x => x.Thread)
              .Returns(() => Observable.Return(new ThreadResponseDTO { Id = 1, Title = "Title 1", Content = "Content 1" }));
            _threadsServiceMock
               .Setup(x => x.GetThread(It.IsAny<int>()))
               .Returns(() => Observable.Return(Unit.Default));
            _threadsServiceMock
               .Setup(x => x.Create(It.IsAny<ThreadRequestDTO>()))
               .Returns(() => Observable.Return(Unit.Default));
        }

        [Test]
        public void AddCommand_ShouldBeDisabled_IfTitleIsEmpty()
        {
            // Arrange
            _target = new AddThreadViewModel(_schedulerService, _viewStackService.Object, _threadsServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.Title = "";
            _target.Content = "Content";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.AddThread.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }

        [Test]
        public void AddCommand_ShouldBeDisabled_IfContentIsEmpty()
        {
            // Arrange
            _target = new AddThreadViewModel(_schedulerService, _viewStackService.Object, _threadsServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.Title = "Title";
            _target.Content = "";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.AddThread.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }

        [Test]
        public void AddCommand_ShouldBeEnabled_IfTitleAndContentAreNotEmpty()
        {
            // Arrange
            _target = new AddThreadViewModel(_schedulerService, _viewStackService.Object, _threadsServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.Title = "Title";
            _target.Content = "Content";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.AddThread.CanExecute.Subscribe(canExecute => Assert.IsTrue(canExecute));
        }

        [Test]
        public void WhenExecuted_CheckIfPopPageIsCalled()
        {
            // Arrange
            _target = new AddThreadViewModel(_schedulerService, _viewStackService.Object, _threadsServiceMock.Object);
            _target.Title = "Title";
            _target.Content = "Content";

            // Act
            Observable.Return(Unit.Default).InvokeCommand(_target.AddThread);

            // Assert
            _threadsServiceMock
                .Verify(x => x.Create(
                    It.IsAny<ThreadRequestDTO>()),
                    Times.Once);
        }
    }
}
