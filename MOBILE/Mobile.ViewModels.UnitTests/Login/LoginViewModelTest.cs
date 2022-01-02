using Mobile.Core.Services.AuthenticationService;
using Mobile.ViewModels.Login;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.UnitTests.Login
{
    [TestFixture]
    public class LoginViewModelTest : ViewModelTestBase
    {
        private LoginViewModel _target;
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
        }

        [Test]
        public void LoginCommand_ShouldBeDisabled_IfEmailIsEmpty()
        {
            // Act
            _target = new LoginViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();
            _target.Password = "Password";
            _target.Email = "";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.Login.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }


        [Test]
        public void LoginCommand_ShouldBeDisabled_IfPasswordIsEmpty()
        {
            // Act
            _target = new LoginViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();
            _target.Password = "";
            _target.Email = "Email";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.Login.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }

        [Test]
        public void LoginCommand_ShouldBeEnabled_IfPasswordAndEmailAreNotEmpty()
        {
            // Act
            _target = new LoginViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();
            _target.Password = "Password";
            _target.Email = "Email";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.Login.CanExecute.Subscribe(canExecute => Assert.IsTrue(canExecute));
        }

        [Test]
        public void LoginCommand_ShouldCallAuthService_Login()
        {
            // Arrange
            _target = new LoginViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();
            _target.Password = "Password";
            _target.Email = "Email";

            _schedulerService.AdvanceBy(1000);

            // Act
            Observable.Return(Unit.Default).InvokeCommand(_target.Login);

            _schedulerService.AdvanceBy(1000);

            // Assert
            _authenticationServiceMock.Verify(x => x.Login(_target.Email, _target.Password), Times.Once);
        }

        [Test]
        public void NavigateToRegisterCommand_ShouldCall_ViewStackPushPage()
        {
            // Arrange
            _target = new LoginViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();

            _schedulerService.AdvanceBy(1000);

            // Act
            Observable.Return(Unit.Default).InvokeCommand(_target.NavigateToRegistrationPage);

            _schedulerService.AdvanceBy(1000);

            // Assert
            _viewStackService.Verify(x => x.PushPage(
              It.IsAny<RegisterViewModel>(),
                     It.IsAny<string>(),
                     It.IsAny<bool>(),
                     It.IsAny<bool>()),
             Times.Once);
        }
    }
}
