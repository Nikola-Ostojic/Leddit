using DynamicData;
using Mobile.Core.Dtos.Response;
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
    public class ThreadViewModelTest : ViewModelTestBase
    {
        private ThreadViewModel _target;
        private Mock<IThreadService> _threadServiceMock;

        [SetUp]
        public void SetUp()
        {
            _threadServiceMock = new Mock<IThreadService>() { DefaultValue = DefaultValue.Mock };

            _threadServiceMock
                .Setup(x => x.Threads)
                .Returns(new SourceCache<ThreadResponseDTO, int>(x => x.Id));
            _threadServiceMock
               .Setup(x => x.Thread)
               .Returns(() => Observable.Return(new ThreadResponseDTO { Id = 1, Title = "Thread1", Content = "Thread1 content", Author = "Author1", CommentsCount = 1 }));
            _threadServiceMock
                .Setup(x => x.GetThreads(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() => Observable.Return(Unit.Default));
            _threadServiceMock
               .Setup(x => x.GetThread(It.IsAny<int>()))
               .Returns(() => Observable.Return(Unit.Default));

            Locator.CurrentMutable.Register(() => _threadServiceMock.Object, typeof(IThreadService));

        }

        [Test]
        public void GetThreads_ShouldCall_IThreadApi_WithSearchCriteria()
        {
            // Arrange
            _target = new ThreadViewModel(_schedulerService, _viewStackService.Object, _threadServiceMock.Object);

            // Act
            _target.Activator.Activate();
            _target.SearchCriteria = "Thread1";

            Observable.Return(25).InvokeCommand(_target.GetThreads);

            // Assert
            _threadServiceMock.Verify(x => x.GetThreads("Thread1", 25), Times.Once);
        }

        [Test]
        public void ShouldCallPushPage_ToAddThread_WhenAddIsClicked()
        {
            // Arrange
            _target = new ThreadViewModel(_schedulerService, _viewStackService.Object, _threadServiceMock.Object);

            // Act
            _schedulerService.AdvanceBy(TimeSpan.FromSeconds(3));

            Observable.Return(Unit.Default).InvokeCommand(_target.ClickAddThread);

            // Assert
            _viewStackService.Verify(x => x.PushPage(
                 It.IsAny<AddThreadViewModel>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<bool>()),
                Times.Exactly(1));
        }
    }
}
