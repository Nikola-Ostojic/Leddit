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
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private string email;
        public string Email
        {
            get => email;
            set => this.RaiseAndSetIfChanged(ref email, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }

        public ReactiveCommand<Unit, bool> Login { get; protected set; }
        public ReactiveCommand<Unit, Unit> NavigateToRegistrationPage { get; protected set; }

        public LoginViewModel(
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IAuthenticationService authenticationService = null) : base(schedulerService, viewStackService)
        {
            _authenticationService = authenticationService ?? Locator.Current.GetService<IAuthenticationService>();

            var canLogin = this
                .WhenAnyValue(x => x.Email, x => x.Password,
                    (email, password) => (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password)));

            Login = ReactiveCommand
                .CreateFromObservable(() => _authenticationService.Login(Email, Password),
                    canLogin,
                    _schedulerService.MainScheduler);

            NavigateToRegistrationPage = ReactiveCommand
                .CreateFromObservable(() => _viewStackService.PushPage(new RegisterViewModel()));

            this.WhenAnyObservable(
                   x => x.Login.IsExecuting)
               .ObserveOn(_schedulerService.MainScheduler)
               .Subscribe(isRunning => IsRunning = isRunning);

            Login
                .ObserveOn(_schedulerService.MainScheduler)
                .Subscribe(isLoggedIn =>
                {
                    if (isLoggedIn)
                    {
                        Observable
                            .Return(Unit.Default)
                            .ObserveOn(_schedulerService.MainScheduler)
                            .SelectMany(_ => _viewStackService.PushPage(new BottomTabNavigationViewModel(), resetStack: true))
                            .Subscribe();
                    }
                });

            Login
                .ThrownExceptions
                .SelectMany(ex => ShowGenericError("Invalid credentials", ex))
                .Subscribe();
        }
    }
}
