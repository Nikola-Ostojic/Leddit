using Mobile.ViewModels.Thread;
using NUnit.Framework;
using System;

namespace Mobile.ViewModels.UnitTests.Thread
{
    public class ThreadCellViewModelTest
    {
        [Test]
        public void ShouldInitializeCellViewModel_WithProvidedValues()
        {
            // Arrange
            var id = 1;
            var title = "Thread1";
            var content = "Thread1 content";
            var author = "Author1";
            var createdAt = new DateTime(1000, 1, 1);
            var modifiedAt = new DateTime(1000, 1, 1);
            var commentsCount = 1;

            // Act
            var vm = new ThreadCellViewModel(id, title, content, author, createdAt, modifiedAt, commentsCount);

            // Assert
            Assert.AreEqual(id, vm.Id);
            Assert.AreEqual(title, vm.Title);
            Assert.AreEqual(content, vm.Content);
            Assert.AreEqual(author, vm.Author);
            Assert.AreEqual(createdAt, vm.CreatedAt);
            Assert.AreEqual(modifiedAt, vm.ModifiedAt);
            Assert.AreEqual(commentsCount, vm.CommentsCount);
        }
    }
}
