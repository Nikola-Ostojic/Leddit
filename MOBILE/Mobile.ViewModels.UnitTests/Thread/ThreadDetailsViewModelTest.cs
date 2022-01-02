using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using Mobile.Core.Services.CommentService;
using Mobile.Core.Services.ThreadService;
using Mobile.ViewModels.Thread;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.UnitTests.Thread
{
    [TestFixture]
    public class ThreadDetailsViewModelTest : ViewModelTestBase
    {
        private ThreadDetailsViewModel _target;
        private Mock<IThreadService> _threadServiceMock;
        private Mock<ICommentService> _commentServiceMock;

        [SetUp]
        public void SetUp()
        {
            _threadServiceMock = new Mock<IThreadService>() { DefaultValue = DefaultValue.Mock };
            _commentServiceMock = new Mock<ICommentService>() { DefaultValue = DefaultValue.Mock };

            _threadServiceMock
               .Setup(x => x.Thread)
               .Returns(() => Observable.Return(new ThreadResponseDTO { Id = 1, Title = "Thread1", Content = "Thread1 content", Author = "Author1", CommentsCount = 1 }));
            _threadServiceMock
               .Setup(x => x.GetThread(It.IsAny<int>()))
               .Returns(() => Observable.Return(Unit.Default));
            _threadServiceMock
                 .Setup(x => x.Delete(It.IsAny<int>()))
                 .Returns(() => Observable.Return(It.IsAny<bool>()));


            _commentServiceMock
                .Setup(x => x.GetComments(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => Observable.Return(Unit.Default));
            _commentServiceMock
                .Setup(x => x.Create(It.IsAny<CommentRequestDTO>()))
               .Returns(() => Observable.Return(Unit.Default));
            _commentServiceMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(() => Observable.Return(It.IsAny<bool>()));

            Locator.CurrentMutable.Register(() => _threadServiceMock.Object, typeof(IThreadService));
        }

        [Test]
        public void CallGetThread_OnActivation()
        {
            // Arrange
            _target = new ThreadDetailsViewModel(1, _schedulerService, _viewStackService.Object, _threadServiceMock.Object, commentService: _commentServiceMock.Object);

            // Act
            _target.Activator.Activate();

            // Assert
            _threadServiceMock.Verify(x => x.GetThread(1), Times.Once);
        }

        [Test]
        public void CallDeleteThread_OnActivation()
        {
            // Arrange
            _target = new ThreadDetailsViewModel(1, _schedulerService, _viewStackService.Object, _threadServiceMock.Object, commentService: _commentServiceMock.Object);

            // Act
            _target.Activator.Activate();

            // Assert
            _threadServiceMock.Verify(x => x.Delete(1), Times.AtMostOnce);
        }

        [Test]
        public void GetComments_UpdatesTheCommentsCache()
        {
            // Arrange
            _target = new ThreadDetailsViewModel(1, _schedulerService, _viewStackService.Object, _threadServiceMock.Object, commentService: _commentServiceMock.Object);

            // Act
            Observable.Return(25).InvokeCommand(_target.GetComments);
            _schedulerService.AdvanceBy(1000);

            // Assert
            _commentServiceMock.Verify(x => x.GetComments(1, 25), Times.AtLeastOnce);
        }

        [Test]
        public void AddCommand_ShouldBeDisabled_IfCommentTextIsEmpty()
        {
            // Arrange
            _target = new ThreadDetailsViewModel(1, _schedulerService, _viewStackService.Object, _threadServiceMock.Object, commentService: _commentServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.CommentText = "";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.AddComment.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }

        [Test]
        public void AddCommand_ShouldBeEnabled_IfCommentTextIsNotEmpty()
        {
            // Arrange
            _target = new ThreadDetailsViewModel(1, _schedulerService, _viewStackService.Object, _threadServiceMock.Object, commentService: _commentServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.CommentText = "Comment text 1";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.AddComment.CanExecute.Subscribe(canExecute => Assert.IsTrue(canExecute));
        }

        [Test]
        public void CallDeleteComment_OnActivation()
        {
            // Arrange
            _target = new ThreadDetailsViewModel(1, _schedulerService, _viewStackService.Object, _threadServiceMock.Object, commentService: _commentServiceMock.Object);

            // Act
            Observable.Return(25).InvokeCommand(_target.GetComments);
            _schedulerService.AdvanceBy(1000);

            // Assert
            _commentServiceMock.Verify(x => x.Delete(1), Times.AtMostOnce);
        }

        [Test]
        public void WhenExecuted_CheckIfPopPageIsCalled()
        {
            // Arrange
            _target = new ThreadDetailsViewModel(1, _schedulerService, _viewStackService.Object, _threadServiceMock.Object, commentService: _commentServiceMock.Object);

           // _target.Title = "Title";
           // _target.Content = "Content";

            // Act
            Observable.Return(Unit.Default).InvokeCommand(_target.DeleteThread);

            // Assert
            _threadServiceMock
                .Verify(x => x.Delete(
                    It.IsAny<int>()),
                    Times.Once);
        }
    }
}
