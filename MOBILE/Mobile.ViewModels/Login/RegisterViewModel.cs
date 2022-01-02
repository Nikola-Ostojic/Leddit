using Mobile.Core.Services.AuthenticationService;
using Mobile.Core.Services.Scheduler;
using Mobile.ViewModels.BottomTabNavigation;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.Login
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private string email;
        public string Email
        {
            get => email;
            set => this.RaiseAndSetIfChanged(ref email, value);
        }

        private string userName;
        public string UserName
        {
            get => userName;
            set => this.RaiseAndSetIfChanged(ref userName, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => this.RaiseAndSetIfChanged(ref confirmPassword, value);
        }

        public ReactiveCommand<Unit, bool> Register { get; protected set; }

        public RegisterViewModel(
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IAuthenticationService authenticationService = null) : base(schedulerService, viewStackService)
        {
            _authenticationService = authenticationService ?? Locator.Current.GetService<IAuthenticationService>(); ;

            var carRegister = this
                .WhenAnyValue(x => x.Email, x => x.UserName, x => x.Password, x => x.ConfirmPassword,
                    (email, username, password, confirmPassword) =>
                    (!string.IsNullOrWhiteSpace(email) &&
                    !string.IsNullOrWhiteSpace(username) &&
                    !string.IsNullOrWhiteSpace(password) &&
                    !string.IsNullOrWhiteSpace(confirmPassword) &&
                    password == confirmPassword));

            Register = ReactiveCommand
                .CreateFromObservable(() => _authenticationService.Register(UserName, Email, Password, ConfirmPassword),
                    carRegister,
                    _schedulerService.MainScheduler);

            this.WhenAnyObservable(
                   x => x.Register.IsExecuting)
               .ObserveOn(_schedulerService.MainScheduler)
               .Subscribe(isRunning => IsRunning = isRunning);

            Register
                .ObserveOn(_schedulerService.MainScheduler)
                .Subscribe(isRegistrationSuccessful =>
                {
                    if (isRegistrationSuccessful)
                    {
                        Observable
                             .Return(Unit.Default)
                             .ObserveOn(_schedulerService.MainScheduler)
                             .SelectMany(_ => _viewStackService.PushPage(new BottomTabNavigationViewModel(), resetStack: true))
                             .Subscribe();
                    }
                });

            Register
                .ThrownExceptions
                .SelectMany(ex => ShowGenericError("Failed to register", ex))
                .Subscribe();
        }
    }
}
