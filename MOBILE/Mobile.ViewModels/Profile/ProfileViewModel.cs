using Mobile.Core.Runtime;
using Mobile.Core.Services.AuthenticationService;
using Mobile.Core.Services.Scheduler;
using Mobile.ViewModels.BottomTabNavigation;
using Mobile.ViewModels.Login;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.Profile
{
    public class ProfileViewModel : ViewModelBase, ITabViewModel
    {
        private readonly IRuntimeContext _runtimeContext;
        private readonly IAuthenticationService _authenticationService;

        public string TabTitle => "Profile";
        public string TabIcon => "TabProfile.png";

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

        public ReactiveCommand<Unit, bool> Logout { get; protected set; }

        public ProfileViewModel(
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IAuthenticationService authenticationService = null,
            IRuntimeContext runtimeContext = null) : base(schedulerService, viewStackService)
        {
            _runtimeContext = runtimeContext ?? Locator.Current.GetService<IRuntimeContext>();
            _authenticationService = authenticationService ?? Locator.Current.GetService<IAuthenticationService>();

            UserName = _runtimeContext.UserName;
            Email = _runtimeContext.Email;

            Logout = ReactiveCommand
              .CreateFromObservable(() => _authenticationService.Logout(), outputScheduler: _schedulerService.MainScheduler);

            Logout
                .ObserveOn(_schedulerService.MainScheduler)
                .Subscribe(_loggedOut =>
                {
                    _viewStackService.PushPage(new LoginViewModel(), resetStack: true).Subscribe();
                });

            Logout
                .ThrownExceptions
                .SelectMany(ex => ShowGenericError("Failed to logout", ex))
                .Subscribe();
        }
    }
}
