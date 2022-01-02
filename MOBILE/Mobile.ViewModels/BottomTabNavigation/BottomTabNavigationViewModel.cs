using Mobile.Core.Services.Scheduler;
using Mobile.ViewModels.Movies;
using Mobile.ViewModels.Profile;
using Mobile.ViewModels.Thread;
using ReactiveUI;
using Sextant.Abstraction;
using System;
using System.Collections.Generic;

namespace Mobile.ViewModels.BottomTabNavigation
{
    public class BottomTabNavigationViewModel : ViewModelBase
    {
        public List<Func<IViewStackService, ITabViewModel>> TabViewModels
        {
            get => _tabViewModels;
            set => this.RaiseAndSetIfChanged(ref _tabViewModels, value);
        }

        private List<Func<IViewStackService, ITabViewModel>> _tabViewModels;

        public readonly List<IViewStackService> _tabStackServices = new List<IViewStackService>();
        public BottomTabNavigationViewModel(
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null) : base(schedulerService, viewStackService)
        {
            TabViewModels = new List<Func<IViewStackService, ITabViewModel>>()
            {
                (customViewStack) =>
                {
                    _tabStackServices.Add(customViewStack);
                    return new ThreadViewModel();
                },
                (customViewStack) =>
                {
                    _tabStackServices.Add(customViewStack);
                    return new MoviesViewModel();
                },
                 (customViewStack) =>
                {
                    _tabStackServices.Add(customViewStack);
                    return new ProfileViewModel();
                },
            };
        }
    }
}
