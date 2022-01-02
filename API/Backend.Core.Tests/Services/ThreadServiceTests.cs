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
    public class ThreadServiceTests
    {
        private readonly IThreadService _threadService;
        private readonly Mock<IThreadRepository> _threadRepositoryMock;

        public ThreadServiceTests()
        {
            _threadRepositoryMock = new Mock<IThreadRepository>();
            _threadService = new ThreadService(_threadRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ThreadDoesntExist_ShouldReturnNull()
        {
            // Arrange
            var id = 1;

            _threadRepositoryMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(() => null);

            // Act
            var result = await _threadService.Get(id);

            // Assert
            Assert.Null(result);
            _threadRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task Get_ThreadExists_ShouldReturnThread()
        {
            // Arrange
            var id = 1;
            var threadEntity = new ThreadEntity
            {
                Id = id,
                Title = "Thread1",
                Content = "Thread1 content",
                Author = new UserEntity(),
                Comments = new List<CommentEntity>()
            };

            _threadRepositoryMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(() => threadEntity);

            // Act
            var result = await _threadService.Get(id);

            // Assert
            Assert.NotNull(result);
            _threadRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task GetWithoutNavigationProperties_ThreadDoesntExist_ShouldReturnNull()
        {
            // Arrange
            var id = 1;

            _threadRepositoryMock
                .Setup(x => x.GetWithoutNavigationProperties(id))
                .ReturnsAsync(() => null);

            // Act
            var result = await _threadService.GetWithoutUser(id);

            // Assert
            Assert.Null(result);
            _threadRepositoryMock.Verify(x => x.GetWithoutNavigationProperties(id), Times.Once);
        }

        [Fact]
        public async Task GetWithoutNavigationProperties_ThreadExists_ShouldReturnThread()
        {
            // Arrange
            var id = 1;
            var threadEntity = new ThreadEntity
            {
                Id = id,
                Title = "Thread1",
                Content = "Thread1 content",
                Author = new UserEntity(),
                Comments = new List<CommentEntity>()
            };

            _threadRepositoryMock
                .Setup(x => x.GetWithoutNavigationProperties(id))
                .ReturnsAsync(() => threadEntity);

            // Act
            var result = await _threadService.GetWithoutUser(id);

            // Assert
            Assert.NotNull(result);
            _threadRepositoryMock.Verify(x => x.GetWithoutNavigationProperties(id), Times.Once);
        }

        [Fact]
        public async Task GetThreads_ShouldReturnThreads()
        {
            // Arrange
            //string searchCriteria, int page, int itemsPerPage
            var searchCriteria = "Thread1";
            var page = 0;
            var itemsPerPage = 20;

            _threadRepositoryMock
                .Setup(x => x.GetThreads(searchCriteria, page, itemsPerPage))
                .ReturnsAsync(new Pageable<ThreadEntity>
                {
                    Page = 1,
                    TotalItems = 2,
                    TotalPages = 1,
                    Data = new List<ThreadEntity>
                    {
                        new ThreadEntity
                        {
                            Id= 1,
                            Title = "Thread1",
                            Content = "Thread1 content",
                            Author = new UserEntity(),
                            Comments = new List<CommentEntity>()
                        },
                        new ThreadEntity
                        {
                            Id= 2,
                            Title = "Thread2",
                            Content = "Thread2 content",
                            Author = new UserEntity(),
                            Comments = new List<CommentEntity>()
                        },
                    }
                });

            // Act
            var result = await _threadService.GetThreads(searchCriteria, page, itemsPerPage);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(1, result.Page);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async Task Create_ShouldReturnTheCreatedThread()
        {
            // Arrange
            var id = 1;
            var threadEntity = new ThreadEntity
            {
                Id = id,
                Title = "Thread1",
                Content = "Thread1 content",
                Author = new UserEntity(),
                Comments = new List<CommentEntity>()
            };

            _threadRepositoryMock
               .Setup(x => x.Create(threadEntity))
               .ReturnsAsync(() => threadEntity);

            // Act
            var result = await _threadService.Create(threadEntity);

            // Assert
            Assert.NotNull(result);
            _threadRepositoryMock.Verify(x => x.Create(threadEntity), Times.Once);
        }

        [Fact]
        public async Task Update_ThreadExists_ShouldReturnTheUpdateThread()
        {
            // Arrange
            var id = 1;
            var threadEntity = new ThreadEntity
            {
                Id = id,
                Title = "Thread1",
                Content = "Thread1 content",
                Author = new UserEntity(),
                Comments = new List<CommentEntity>()
            };

            _threadRepositoryMock
               .Setup(x => x.Update(threadEntity))
               .ReturnsAsync(() => threadEntity);

            // Act
            var result = await _threadService.Update(threadEntity);

            // Assert
            Assert.NotNull(result);
            _threadRepositoryMock.Verify(x => x.Update(threadEntity), Times.Once);
        }

        [Fact]
        public async Task Update_ThreadDoesntExist_ShouldReturnNull()
        {
            // Arrange
            var id = 1;
            var threadEntity = new ThreadEntity
            {
                Id = id,
                Title = "Thread1",
                Content = "Thread1 content",
                Author = new UserEntity(),
                Comments = new List<CommentEntity>()
            };

            _threadRepositoryMock
               .Setup(x => x.Update(threadEntity))
               .ReturnsAsync(() => null);

            // Act
            var result = await _threadService.Update(threadEntity);

            // Assert
            Assert.Null(result);
            _threadRepositoryMock.Verify(x => x.Update(threadEntity), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            var id = 1;
            _threadRepositoryMock
                .Setup(x => x.Delete(id));

            // Act
            _threadService.Delete(id);

            // Assert
            _threadRepositoryMock.Verify(x => x.Delete(id), Times.Once);
        }
    }
}
