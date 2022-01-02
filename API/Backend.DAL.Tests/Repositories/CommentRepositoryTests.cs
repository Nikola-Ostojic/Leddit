using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using Backend.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.DAL.Tests.Repositories
{
    public class CommentRepositoryTests : RepositoryUnitTestsBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentRepositoryTests()
        {
            // Arrange
            _commentRepository = new CommentRepository(DbContext);
            DbContext.Comments.Add(new CommentEntity { Content = "Day is beautiful.", Thread = new ThreadEntity() { Id = 4 }, Author = new UserEntity() });
            DbContext.Comments.Add(new CommentEntity { Content = "It's sunny outside.", Thread = new ThreadEntity() { Id = 5 }, Author = new UserEntity() });

            DbContext.SaveChanges();
        }

        [Fact]
        public async Task GetPeageable_ShouldReturnExpectedResults()
        {
            // Act
            var pageableResult = await _commentRepository.GetComments(0, 0, 0);

            // Assert
            Assert.Equal(2, pageableResult.Data.Count);
            Assert.Equal(2, pageableResult.TotalItems);
            Assert.Equal(1, pageableResult.TotalPages);

            var firstcomment = pageableResult.Data.Last();
            Assert.Equal(1, firstcomment.Id);
            Assert.Equal("Day is beautiful.", firstcomment.Content);
            Assert.NotNull(firstcomment.Author);

            var secondcomment = pageableResult.Data.First();
            Assert.Equal(2, secondcomment.Id);
            Assert.Equal("It's sunny outside.", secondcomment.Content);
            Assert.NotNull(secondcomment.Author);
        }

        [Fact]
        public async Task Search_ShouldFindCommentsOfSameThread()
        {
            var comments = await _commentRepository.GetComments(1, 0, 3);

            Assert.NotNull(comments);
            Assert.Equal(1, comments.TotalPages);
        }

        [Fact]
        public async Task GetCommentById_CommentExists_ReturnComment()
        {
            var comment = await _commentRepository.Get(1);

            Assert.NotNull(comment);
            Assert.Equal(1, comment.Id);
            Assert.Equal("Day is beautiful.", comment.Content);
            Assert.NotNull(comment.Author);
        }

        [Fact]
        public async Task GetCommentById_CommentDoesntExists_ReturnNull()
        {
            var comment = await _commentRepository.Get(3);

            Assert.Null(comment);
        }

        [Fact]
        public async Task GetCommentCount_NumberExists_ReturnNumber()
        {
            var commentCount = await _commentRepository.GetCommentsCount(1);
            Assert.True(commentCount >= 0);
        }

        [Fact]
        public async Task CreateComment_ShouldReturnTheCreatedComment_AndIncreaseTheCommentsCount()
        {
            var newComment = new CommentEntity { Id = DbContext.Comments.Count() + 1, Content = "Work is marvellous." };
            var expectedNumberOfComments = DbContext.Comments.Count() + 1;

            var createdComment = await _commentRepository.Create(newComment);

            Assert.NotNull(createdComment);
            Assert.Equal(newComment.Content, createdComment.Content);
            Assert.True(newComment.Id != 0);
            Assert.Equal(expectedNumberOfComments, DbContext.Comments.Count());
            Assert.Equal(newComment.Author, newComment.Author);
        }

        [Fact]
        public async Task IfCommentExists_UpdateComment_AndReturnUpdatedComment()
        {
            var comment = DbContext.Comments.Find(1);
            comment.Content = "Work is not good.";

            var updatedComment = await _commentRepository.Update(comment);

            Assert.NotNull(updatedComment);
            Assert.Equal(comment.Content, updatedComment.Content);
            Assert.Equal(comment.Id, updatedComment.Id);
            Assert.Equal(comment.Author, updatedComment.Author);
        }

        [Fact]
        public async Task IfTheCommentToUpdate_DoesntExist_ReturnNull()
        {
            var comment = new CommentEntity { Id = 3, Content = "Work is fantastic." };

            var result = await _commentRepository.Update(comment);

            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_ShouldDeleteTheCommentFromTheDatabase()
        {
            var commentId = 1;

            await _commentRepository.Delete(commentId);
            Assert.Equal(1, DbContext.Comments.Count());
        }

        [Fact]
        public async Task Delete_AttemptToDeleteAnEntityThatDoesntExist_ShouldNotChangeTheTotalCount()
        {
            var commentId = 11;

            await _commentRepository.Delete(commentId);
            Assert.Equal(2, DbContext.Comments.Count());
        }
    }
}
