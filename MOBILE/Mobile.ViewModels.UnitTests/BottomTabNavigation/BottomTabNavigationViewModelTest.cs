using Mobile.ViewModels.BottomTabNavigation;
using NUnit.Framework;

namespace Mobile.ViewModels.UnitTests.BottomTabNavigation
{
    [TestFixture]
    class BottomTabNavigationViewModelTest : ViewModelTestBase
    {
        private BottomTabNavigationViewModel _target;

        [Test]
        public void BottomTabNavigation_ShouldContainThree_TabViewModels()
        {
            // Act
            _target = new BottomTabNavigationViewModel(_schedulerService, _viewStackService.Object);

            // Assert
            Assert.AreEqual(3, _target.TabViewModels.Count);
        }
    }
}
