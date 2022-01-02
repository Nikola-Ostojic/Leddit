using Mobile.Core.Runtime;
using Mobile.Core.Services.AuthenticationService;
using Mobile.ViewModels.BottomTabNavigation;
using Mobile.ViewModels.Login;
using Moq;
using NUnit.Framework;
using System;
using System.Reactive.Linq;

namespace Mobile.ViewModels.UnitTests.Login
{
    [TestFixture]

    public class AutoLoginViewModelTest : ViewModelTestBase
    {
        private AutoLoginViewModel _target;
        private Mock<IAuthenticationService> _authenticationServiceMock;

        [SetUp]
        public void SetUp()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>() { DefaultValue = DefaultValue.Mock };
            _runtimeContextMock = new Mock<IRuntimeContext>() { DefaultValue = DefaultValue.Mock };

            _authenticationServiceMock
                .Setup(x => x.IsTokenExpired(It.IsAny<string>()))
                .Returns(() => Observable.Return(true));
            _authenticationServiceMock
               .Setup(x => x.RenewSession(It.IsAny<string>()))
               .Returns(() => Observable.Return(true));
            _runtimeContextMock
                .Setup(x => x.AccessToken)
                .Returns(() => null);
        }

        [Test]
        public void ShouldPushTo_LoginViewModel_IfRuntimeContextDoesNotHaveAccessAndRefreshToken()
        {
            // Arrange
            _runtimeContextMock
               .Setup(x => x.AccessToken)
               .Returns(() => null);
            _runtimeContextMock
              .Setup(x => x.RefreshToken)
              .Returns(() => null);

            // Act
            _target = new AutoLoginViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object, _runtimeContextMock.Object);

            _target.Activator.Activate();

            // Assert
            _viewStackService.Verify(x => x.PushPage(
               It.IsAny<LoginViewModel>(),
                      It.IsAny<string>(),
                      It.IsAny<bool>(),
                      It.IsAny<bool>()),
              Times.Exactly(1));
        }

        [Test]
        public void ShouldNavigateToBottomTabNavigationViewModel_IfTokenIsSuccessfulyRefreshed()
        {
            // Arrange
            _runtimeContextMock
              .Setup(x => x.AccessToken)
              .Returns(() => "access");
            _runtimeContextMock
              .Setup(x => x.RefreshToken)
              .Returns(() => "refresh");

            // Act
            _target = new AutoLoginViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object, _runtimeContextMock.Object);

            _target.Activator.Activate();

            // Assert
            _authenticationServiceMock.Verify(x => x.IsTokenExpired(
                      It.IsAny<string>()),
              Times.Exactly(1));
            _schedulerService.AdvanceBy(TimeSpan.FromSeconds(3));
            _authenticationServiceMock.Verify(x => x.RenewSession(
                      It.IsAny<string>()),
              Times.Exactly(1));
            _viewStackService.Verify(x => x.PushPage(
              It.IsAny<BottomTabNavigationViewModel>(),
                     It.IsAny<string>(),
                     It.IsAny<bool>(),
                     It.IsAny<bool>()),
             Times.Exactly(1));
        }

        [Test]
        public void ShouldNavigateToLoginPage_IfTokenCanNotBeRenewed()
        {
            // Arrange
            _runtimeContextMock
              .Setup(x => x.AccessToken)
              .Returns(() => "access");
            _runtimeContextMock
              .Setup(x => x.RefreshToken)
              .Returns(() => "refresh");
            _authenticationServiceMock
            .Setup(x => x.RenewSession(It.IsAny<string>()))
            .Returns(() => Observable.Return(false));

            // Act
            _target = new AutoLoginViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object, _runtimeContextMock.Object);

            _target.Activator.Activate();

            // Assert
            _authenticationServiceMock.Verify(x => x.IsTokenExpired(
                      It.IsAny<string>()),
              Times.Exactly(1));
            _schedulerService.AdvanceBy(TimeSpan.FromSeconds(3));
            _authenticationServiceMock.Verify(x => x.RenewSession(
                      It.IsAny<string>()),
              Times.Exactly(1));
            _viewStackService.Verify(x => x.PushPage(
              It.IsAny<LoginViewModel>(),
                     It.IsAny<string>(),
                     It.IsAny<bool>(),
                     It.IsAny<bool>()),
             Times.Exactly(1));
        }
    }
}
