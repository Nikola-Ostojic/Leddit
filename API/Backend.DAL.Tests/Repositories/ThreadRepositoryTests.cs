using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using Backend.DAL.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.DAL.Tests.Repositories
{
    public class ThreadRepositoryTests : RepositoryUnitTestsBase
    {
        private readonly IThreadRepository _threadRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly Mock<ICommentRepository> commentsMock;
        public ThreadRepositoryTests()
        {
            // Arrange
            var user = new UserEntity();
            var comments = new List<CommentEntity>() { new CommentEntity() };
            commentsMock = new Mock<ICommentRepository>();
            _threadRepository = new ThreadRepository(DbContext, commentsMock.Object);
            _commentRepository = new CommentRepository(DbContext);
            DbContext.Threads.Add(new ThreadEntity { Title = "Thread1", Content = "Thread1 content", Author = user, Comments = comments });
            DbContext.Threads.Add(new ThreadEntity { Title = "Thread2", Content = "Thread2 content", Author = user, Comments = comments });
            DbContext.SaveChanges();
        }


        [Fact]
        public async Task GetPeageable_ShouldReturnExpectedResults()
        {
            // Act
            var pageableResult = await _threadRepository.GetThreads("", 0, 0);

            // Assert
            Assert.Equal(2, pageableResult.Data.Count);
            Assert.Equal(2, pageableResult.TotalItems);
            Assert.Equal(1, pageableResult.TotalPages);

            var firstThread = pageableResult.Data.Last();
            Assert.Equal(1, firstThread.Id);
            Assert.Equal("Thread1", firstThread.Title);
            Assert.Equal("Thread1 content", firstThread.Content);
            Assert.NotNull(firstThread.Author);
            Assert.True(firstThread.Comments.Count > 0);

            var secondThread = pageableResult.Data.First();
            Assert.Equal(2, secondThread.Id);
            Assert.Equal("Thread2", secondThread.Title);
            Assert.Equal("Thread2 content", secondThread.Content);
            Assert.NotNull(secondThread.Author);
            Assert.True(secondThread.Comments.Count > 0);
        }

        [Fact]
        public async Task GetPeageable_ShouldReturnExpectedResults_WithFilterForTitle()
        {
            // Act
            var pageableResult = await _threadRepository.GetThreads("Thread1", 0, 0);

            // Assert
            Assert.Single(pageableResult.Data);
            Assert.Equal(1, pageableResult.TotalItems);
            Assert.Equal(1, pageableResult.TotalPages);

            var firstThread = pageableResult.Data.First();
            Assert.Equal(1, firstThread.Id);
            Assert.Equal("Thread1", firstThread.Title);
            Assert.Equal("Thread1 content", firstThread.Content);
            Assert.NotNull(firstThread.Author);
            Assert.True(firstThread.Comments.Count > 0);
        }

        [Fact]
        public async Task GetPeageable_ShouldReturnExpectedResults_WithFilterForContent()
        {
            // Act
            var pageableResult = await _threadRepository.GetThreads("Thread1 content", 0, 0);

            // Assert
            Assert.Single(pageableResult.Data);
            Assert.Equal(1, pageableResult.TotalItems);
            Assert.Equal(1, pageableResult.TotalPages);

            var firstThread = pageableResult.Data.First();
            Assert.Equal(1, firstThread.Id);
            Assert.Equal("Thread1", firstThread.Title);
            Assert.Equal("Thread1 content", firstThread.Content);
            Assert.NotNull(firstThread.Author);
            Assert.True(firstThread.Comments.Count > 0);
        }

        [Fact]
        public async Task GetThreadById_ThreadExists_ReturnThread()
        {
            var thread = await _threadRepository.Get(1);

            Assert.NotNull(thread);
            Assert.Equal(1, thread.Id);
            Assert.Equal("Thread1", thread.Title);
            Assert.Equal("Thread1 content", thread.Content);
            Assert.NotNull(thread.Author);
        }

        [Fact]
        public async Task GetThreadById_ThreadDoesntExists_ReturnNull()
        {
            var thread = await _threadRepository.Get(3);

            Assert.Null(thread);
        }

        [Fact]
        public async Task CreateThread_ShouldReturnTheCreatedThread_AndIncreaseTheThreadsCount()
        {
            var newThread = new ThreadEntity
            {
                Title = "Thread3",
                Content = "Thread 3 content",
                Author = new UserEntity(),
                Comments = new List<CommentEntity>() 
            };

            var expectedNumberOfThreads = DbContext.Threads.Count() + 1;

            var createdThread = await _threadRepository.Create(newThread);

            Assert.NotNull(createdThread);
            Assert.Equal(newThread.Title, createdThread.Title);
            Assert.Equal(newThread.Content, createdThread.Content);
            Assert.Equal(newThread.Author, createdThread.Author);
            Assert.Equal(newThread.Comments, createdThread.Comments);
            Assert.True(newThread.Id != 0);
            Assert.Equal(expectedNumberOfThreads, DbContext.Threads.Count());
        }

        [Fact]
        public async Task IfThreadExists_UpdateThread_AndReturnUpdatedThread()
        {
            var thread = DbContext.Threads.Find(1);
            thread.Title = "Thread4";
            thread.Content = "Thread 4 content";
            thread.Author = new UserEntity();
            thread.Comments = new List<CommentEntity>();

            var updatedThread = await _threadRepository.Update(thread);

            Assert.NotNull(updatedThread);
            Assert.Equal(thread.Title, updatedThread.Title);
            Assert.Equal(thread.Content, updatedThread.Content);
            Assert.Equal(thread.Author, updatedThread.Author);
            Assert.Equal(thread.Comments, updatedThread.Comments);
            Assert.Equal(thread.Id, updatedThread.Id);
        }

        [Fact]
        public async Task IfTheThreadToUpdate_DoesntExist_ReturnNull()
        {
            var thread = new ThreadEntity { Id = 3, Title = "Thread5" };

            var result = await _threadRepository.Update(thread);

            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_ShouldDeleteTheThreadFromTheDatabase()
        {
            var threadId = 1;

            await _threadRepository.Delete(threadId);
            Assert.Equal(1, DbContext.Threads.Count());
            commentsMock
               .Setup(x => x.Get(threadId))
               .ReturnsAsync(() => null);

            commentsMock.Verify(x => x.DeleteCommentForThread(threadId));
        }

        [Fact]
        public async Task Delete_AttemptToDeleteAnEntityThatDoesntExist_ShouldNotChangeTheTotalCount()
        {
            var threadId = 11;

            await _threadRepository.Delete(threadId);
            Assert.Equal(2, DbContext.Threads.Count());
        }
    }
}
