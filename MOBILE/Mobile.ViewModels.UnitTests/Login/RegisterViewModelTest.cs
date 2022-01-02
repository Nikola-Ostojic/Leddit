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

    class RegisterViewModelTest : ViewModelTestBase
    {
        private RegisterViewModel _target;
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
        public void RegisterCommand_ShouldBeDisabled_IfEmailIsEmpty()
        {
            // Act
            _target = new RegisterViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();
            _target.Password = "Password";
            _target.Email = "";
            _target.ConfirmPassword = "";
            _target.UserName = "";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.Register.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }


        [Test]
        public void RegisterCommand_ShouldBeDisabled_IfPasswordIsEmpty()
        {
            // Arrange
            _target = new RegisterViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();
            _target.Password = "";
            _target.Email = "Email";
            _target.ConfirmPassword = "";
            _target.UserName = "";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.Register.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }

        [Test]
        public void RegisterCommand_ShouldBeDisabled_IfAllFieldsAreFilledButPasswordAndConfirmationPasswordDoNotMatch()
        {
            // Arrange
            _target = new RegisterViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();
            _target.Password = "Password";
            _target.Email = "Email";
            _target.ConfirmPassword = "One";
            _target.UserName = "Two";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.Register.CanExecute.Subscribe(canExecute => Assert.IsFalse(canExecute));
        }

        [Test]
        public void RegisterCommand_ShouldBeDisabled_IfAllFieldsAreFilled_PasswordAndConfirmationPasswordMatch()
        {
            // Act
            _target = new RegisterViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();
            _target.Password = "Password";
            _target.ConfirmPassword = "Password";
            _target.Email = "Email";
            _target.UserName = "UserName";

            _schedulerService.AdvanceBy(1000);

            // Assert
            _target.Register.CanExecute.Subscribe(canExecute => Assert.IsTrue(canExecute));
        }

        [Test]
        public void RegisterCommand_ShouldCallAuthService_Register()
        {
            // Arrange
            _target = new RegisterViewModel(_schedulerService, _viewStackService.Object, _authenticationServiceMock.Object);
            _target.Activator.Activate();
            _target.Password = "Password";
            _target.ConfirmPassword = "Password";
            _target.Email = "Email";
            _target.UserName = "UserName";

            _schedulerService.AdvanceBy(1000);

            // Act
            Observable.Return(Unit.Default).InvokeCommand(_target.Register);

            _schedulerService.AdvanceBy(1000);

            // Assert
            _authenticationServiceMock.Verify(x => x.Register(_target.UserName, _target.Email, _target.Password, _target.ConfirmPassword), Times.Once);
        }
    }
}
