using Mobile.Core.Services.AuthenticationService;
using Mobile.ViewModels.Profile;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.UnitTests.Profile
{
    [TestFixture]

    class ProfileViewModelTest : ViewModelTestBase
    {
        private ProfileViewModel _target;
        private Mock<IAuthenticationService> _authenticationServiceMock;

        [SetUp]
        public void SetUp()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>() { DefaultValue = DefaultValue.Mock };

            _authenticationServiceMock
                .Setup(x => x.IsTokenExpired(It.IsAny<string>()))
                .Returns(() => Observable.Return(true));
            _authenticationServiceMock
               .Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(() => Observable.Return(true));
            _authenticationServiceMock
               .Setup(x => x.RenewSession(It.IsAny<string>()))
               .Returns(() => Observable.Return(true));
            _runtimeContextMock
                .Setup(x => x.AccessToken)
                .Returns(() => null);
            _runtimeContextMock
                .Setup(x => x.UserName)
                .Returns("User");
            _runtimeContextMock
                .Setup(x => x.Email)
                .Returns("Email");
        }

        [Test]
        public void UserNameAndEmail_ShouldBeMatchingRuntimeContext()
        {
            // Act
            _target = new ProfileViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object, _runtimeContextMock.Object);

            // Assert
            Assert.AreEqual(_target.Email, _runtimeContextMock.Object.Email);
            Assert.AreEqual(_target.UserName, _runtimeContextMock.Object.UserName);
        }

        [Test]
        public void LogoutCommand_ShouldCallAuthenticationService_Logout()
        {
            // Arrange
            _target = new ProfileViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object, _runtimeContextMock.Object);

            // Act
            Observable.Return(Unit.Default).InvokeCommand(_target.Logout);

            _schedulerService.AdvanceBy(1000);

            // Assert
            _authenticationServiceMock.Verify(x => x.Logout(), Times.Once);

        }
    }
}
