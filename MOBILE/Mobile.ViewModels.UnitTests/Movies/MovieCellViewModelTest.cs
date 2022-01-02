using Mobile.ViewModels.Movies;
using NUnit.Framework;

namespace Mobile.ViewModels.UnitTests.Movies
{
    [TestFixture]
    public class MovieCellViewModelTest
    {
        [Test]
        public void ShouldInitializeCellViewModel_WithProvidedValues()
        {
            // Arrange
            var id = 1;
            var name = "name";
            var imageUrl = "image Url";

            // Act
            var vm = new MovieCellViewModel(id, name, imageUrl);

            // Assert
            Assert.AreEqual(id, vm.Id);
            Assert.AreEqual(name, vm.Name);
            Assert.AreEqual(imageUrl, vm.ImageUrl);
        }
    }
}
