using Mobile.ViewModels.Thread;
using NUnit.Framework;

namespace Mobile.ViewModels.UnitTests.Comment
{
    public class CommentCellViewModelTest
    {
        [Test]
        public void ShouldInitializeCellViewModel_WithProvidedValues()
        {
            // Arrange
            var id = 1;
            var content = "Comment1 content";
            var author = "Author1";

            // Act
            var vm = new CommentCellViewModel(id, author, content);

            // Assert
            Assert.AreEqual(id, vm.Id);
            Assert.AreEqual(content, vm.Content);
            Assert.AreEqual(author, vm.Author);
        }
    }
}
