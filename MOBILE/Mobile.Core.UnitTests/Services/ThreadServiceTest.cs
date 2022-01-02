using Mobile.Core.Api;
using Mobile.Core.Api.Rest;
using Mobile.Core.Dtos;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using Mobile.Core.Services.ThreadService;
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
    public class ThreadServiceTest
    {
        private IThreadService _target;
        private Mock<IApiService<IThreadApi>> _threadApiServiceMock;
        private Mock<IThreadApi> _threadApiMock;

        [SetUp]
        public void Setup()
        {
            _threadApiMock = new Mock<IThreadApi>();
            var threadList = new List<ThreadResponseDTO>
            {
                new ThreadResponseDTO
                {
                    Id = 1,
                    Title = "Thread1",
                    Content = "Thread1 content",
                    Author = "Author1",
                    CommentsCount = 1

                },
                new ThreadResponseDTO
                {
                    Id = 2,
                    Title = "Thread2",
                    Content = "Thread2 content",
                    Author = "Author2",
                    CommentsCount = 2
                }
            };

            _threadApiMock
                .Setup(x => x.FetchThreads(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() => Observable.Return(new PageableDTO<ThreadResponseDTO>
                {
                    Data = threadList,
                    Page = 1,
                    TotalItems = 2,
                    TotalPages = 1
                })
                );

            _threadApiMock
               .Setup(x => x.FetchThread(It.IsAny<int>()))
               .Returns(() => Observable.Return(threadList.First()));

            _threadApiMock
             .Setup(x => x.DeleteThread(It.IsAny<int>()))
             .Returns(() => Observable.Return("Deleted"));

            _threadApiMock
                .Setup(x => x.CreateThread(It.IsAny<ThreadRequestDTO>()))
                 .Returns(() => Observable.Return(Unit.Default));

            _threadApiServiceMock = new Mock<IApiService<IThreadApi>>();
            _threadApiServiceMock
                .Setup(x => x.GetClient())
                .Returns(() => _threadApiMock.Object);
        }

        [Test]
        public void GetThreads_UpdatesTheThreadsCache()
        {
            // Arrange
            _target = new ThreadService(_threadApiServiceMock.Object);


            // Act
            _target.GetThreads("Title", 25).Subscribe();


            // Assert
            _threadApiMock.Verify(x => x.FetchThreads("Title", 25), Times.Once);
            Assert.AreEqual(2, _target.Threads.Count);
        }

        [Test]
        public void GetThread_SetsTheThread()
        {
            // Arrange
            _target = new ThreadService(_threadApiServiceMock.Object);

            // Act
            _target.GetThread(1).Subscribe();

            // Assert
            _threadApiMock.Verify(x => x.FetchThread(1), Times.Once);
            _target.Thread.Subscribe(m =>
            {
                Assert.AreEqual(1, m.Id);
                Assert.AreEqual("Thread1", m.Title);
                Assert.AreEqual("Thread1 content", m.Content);
            });
        }

        [Test]
        public void DeleteThread_ShouldCallRestDeleteEndpoint()
        {
            // Arrange
            _target = new ThreadService(_threadApiServiceMock.Object);

            // Act
            _target.Delete(1).Subscribe();

            // Assert
            _threadApiMock.Verify(x => x.DeleteThread(1), Times.Once);
        }

        [Test]
        public void AddThread_ShouldCallRestAddEndpoint()
        {
            // Arrange
            _target = new ThreadService(_threadApiServiceMock.Object);

            // Act
            _target.Create(new ThreadRequestDTO()).Subscribe();

            // Assert
            _threadApiMock.Verify(x => x.CreateThread(It.IsAny<ThreadRequestDTO>()), Times.Once);
        }
    }
}
