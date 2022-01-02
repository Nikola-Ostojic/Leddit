using Mobile.Core.Api;
using Mobile.Core.Api.Rest;
using Mobile.Core.Dtos;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using Mobile.Core.Services.CommentService;
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
    public class CommentServiceTest
    {
        private ICommentService _target;
        private Mock<IApiService<ICommentApi>> _commentApiServiceMock;
        private Mock<ICommentApi> _commentApiMock;

        [SetUp]
        public void Setup()
        {
            _commentApiMock = new Mock<ICommentApi>();
            var commentList = new List<CommentResponseDTO>
            {
                new CommentResponseDTO
                {
                    Id = 1,
                    Content = "Comment1 content",
                    Author = "Author1"
                },
                new CommentResponseDTO
                {
                    Id = 2,
                    Content = "Comment2 content",
                    Author = "Author2"
                }
            };

            _commentApiMock
                .Setup(x => x.FetchComments(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => Observable.Return(new PageableDTO<CommentResponseDTO>
                {
                    Data = commentList,
                    Page = 1,
                    TotalItems = 2,
                    TotalPages = 1
                })
                );

            _commentApiMock
               .Setup(x => x.FetchComment(It.IsAny<int>()))
               .Returns(() => Observable.Return(commentList.First()));

            _commentApiMock
             .Setup(x => x.DeleteComment(It.IsAny<int>()))
             .Returns(() => Observable.Return("Deleted"));

            _commentApiMock
                .Setup(x => x.CreateComment(It.IsAny<CommentRequestDTO>()))
                 .Returns(() => Observable.Return(Unit.Default));

            _commentApiServiceMock = new Mock<IApiService<ICommentApi>>();
            _commentApiServiceMock
                .Setup(x => x.GetClient())
                .Returns(() => _commentApiMock.Object);
        }

        [Test]
        public void GetComments_UpdatesTheCommentsCache()
        {
            // Arrange
            _target = new CommentService(_commentApiServiceMock.Object);


            // Act
            _target.GetComments(1, 25).Subscribe();


            // Assert
            _commentApiMock.Verify(x => x.FetchComments(1, 25), Times.Once);
            Assert.AreEqual(2, _target.Comments.Count);
        }

        [Test]
        public void GetComment_SetsTheComment()
        {
            // Arrange
            _target = new CommentService(_commentApiServiceMock.Object);

            // Act
            _target.GetComment(1).Subscribe();

            // Assert
            _commentApiMock.Verify(x => x.FetchComment(1), Times.Once);
            _target.Comment.Subscribe(m =>
            {
                Assert.AreEqual(1, m.Id);
                Assert.AreEqual("Comment 1 author", m.Author);
                Assert.AreEqual("Comment 1 content", m.Content);
            });
        }

        [Test]
        public void DeleteComment_ShouldCallRestDeleteEndpoint()
        {
            // Arrange
            _target = new CommentService(_commentApiServiceMock.Object);

            // Act
            _target.Delete(1).Subscribe();

            // Assert
            _commentApiMock.Verify(x => x.DeleteComment(1), Times.Once);
        }

        [Test]
        public void AddComment_ShouldCallRestAddEndpoint()
        {
            // Arrange
            _target = new CommentService(_commentApiServiceMock.Object);

            // Act
            _target.Create(new CommentRequestDTO()).Subscribe();

            // Assert
            _commentApiMock.Verify(x => x.CreateComment(It.IsAny<CommentRequestDTO>()), Times.Once);
        }
    }
}
