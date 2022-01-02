using Backend.Core.Interfaces;
using Backend.Core.Services;
using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using Backend.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Core.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly ICommentService _commentService;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;

        public CommentServiceTests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _commentService = new CommentService(_commentRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_CommentsExists_ShouldReturnComment()
        {
            // Arrange
            var id = 1;

            _commentRepositoryMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(() => null);

            // Act
            var result = await _commentService.Get(id);

            // Assert
            Assert.Null(result);
            _commentRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task Get_CommentDoesntExist_ShouldReturnNull()
        {
            // Arrange
            var id = 9;
            var commentEntity = new CommentEntity
            {
                Id = 9,
                Content = "Day goes by."
            };

            _commentRepositoryMock
                .Setup(x => x.Get(10))
                .ReturnsAsync(() => commentEntity);



            // Act
            var result = await _commentService.Get(9);

            // Assert
            Assert.Null(result);
            _commentRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        /// <summary>
        /// Not similar to movie test. GetComments works with Thread ids.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetComments_ShouldReturnComments()
        {
            // Arrange
            //int id, int page, int itemsPerPag
            var id = 1;
            var page = 0;
            var itemsPerPage = 20;
            ThreadEntity th1 = new ThreadEntity() { Id = 1 };

            _commentRepositoryMock
                .Setup(x => x.GetComments(id, page, itemsPerPage))
                .ReturnsAsync(new Pageable<CommentEntity>
                {
                    Page = 1,
                    TotalItems = 2,
                    TotalPages = 1,
                    Data = new List<CommentEntity>
                    {
                        new CommentEntity
                        {
                            Id= 1,
                            Thread = th1,
                            Content = "Day is beautiful.",
                        },
                        new CommentEntity
                        {
                            Id= 2,
                            Thread = th1,
                            Content = "Day is not beautiful.",
                        },
                    }
                });

            // Act
            var result = await _commentService.GetComments(id, page, itemsPerPage);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(1, result.Page);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async Task Get_CommentsCountExists_ShouldReturnCommentCount()
        {
            // Arrange
            var id = 1;

            _commentRepositoryMock
                .Setup(x =>  x.GetCommentsCount(id))
                .ReturnsAsync(() => 0);

            // Act
            var result = await _commentService.GetCommentsCount(id);

            // Assert
            Assert.True(result >= 0);
            _commentRepositoryMock.Verify(x => x.GetCommentsCount(id), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnTheCreatedComment()
        {
            // Arrange
            var id = 1;
            var commentEntity = new CommentEntity
            {
                Id = id,
                Content = "Good wheather."
            };

            _commentRepositoryMock
               .Setup(x => x.Create(commentEntity))
               .ReturnsAsync(() => commentEntity);

            // Act
            var result = await _commentService.Create(commentEntity);

            // Assert
            Assert.NotNull(result);
            _commentRepositoryMock.Verify(x => x.Create(commentEntity), Times.Once);
        }

        [Fact]
        public async Task Update_CommentExists_ShouldReturnTheUpdateComment()
        {
            // Arrange
            var id = 1;
            var commentEntity = new CommentEntity
            {
                Id = id,
                Content = "Good work!"
            };

            _commentRepositoryMock
               .Setup(x => x.Update(commentEntity))
               .ReturnsAsync(() => commentEntity);

            // Act
            var result = await _commentService.Update(commentEntity);

            // Assert
            Assert.NotNull(result);
            _commentRepositoryMock.Verify(x => x.Update(commentEntity), Times.Once);
        }

        [Fact]
        public async Task Update_CommentDoesntExist_ShouldReturnNull()
        {
            // Arrange
            var id = 1;
            var commentEntity = new CommentEntity
            {
                Id = id,
                Content = "Good work!"
            };

            _commentRepositoryMock
               .Setup(x => x.Update(commentEntity))
               .ReturnsAsync(() => null);

            // Act
            var result = await _commentService.Update(commentEntity);

            // Assert
            Assert.Null(result);
            _commentRepositoryMock.Verify(x => x.Update(commentEntity), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            var id = 1;
            _commentRepositoryMock
                .Setup(x => x.Delete(id));

            // Act
            _commentService.Delete(id);

            // Assert
            _commentRepositoryMock.Verify(x => x.Delete(id), Times.Once);
        }
    }
}
