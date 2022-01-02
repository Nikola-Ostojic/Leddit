using Mobile.Core.Runtime;
using Mobile.Core.Services.AuthenticationService;
using Mobile.Core.Services.Scheduler;
using Mobile.ViewModels.BottomTabNavigation;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Mobile.ViewModels.Login
{
    public class AutoLoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRuntimeContext _runtimeContext;

        public AutoLoginViewModel(
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IAuthenticationService authenticationService = null,
            IRuntimeContext runtimeContext = null) : base(schedulerService, viewStackService)
        {
            _authenticationService = authenticationService ?? Locator.Current.GetService<IAuthenticationService>();
            _runtimeContext = runtimeContext ?? Locator.Current.GetService<IRuntimeContext>();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                if (_runtimeContext.AccessToken != null && _runtimeContext.RefreshToken != null)
                {
                    IsRunning = true;
                    _authenticationService
                        .IsTokenExpired(_runtimeContext.AccessToken)
                        .SubscribeOn(_schedulerService.TaskPoolScheduler)
                        .SelectMany(_ => _authenticationService
                                            .RenewSession(_runtimeContext.RefreshToken)
                                            .Catch<bool, Exception>(ex => Observable.Return(false)))
                        .Select(isLoggedIn => isLoggedIn)
                        .ObserveOn(_schedulerService.MainScheduler)
                        .Select(isLoggedIn =>
                        {
                            var navigation = isLoggedIn
                          ? _viewStackService.PushPage(new BottomTabNavigationViewModel(), resetStack: true)
                          : _viewStackService.PushPage(new LoginViewModel(), resetStack: true);
                            navigation.Subscribe();

                            return isLoggedIn;
                        })
                        .ObserveOn(_schedulerService.TaskPoolScheduler)
                        .Subscribe((x) => IsRunning = false); ;
                }
                else
                {
                    _viewStackService.PushPage(new LoginViewModel(), resetStack: true).Subscribe();
                }
            });
        }
    }
}
