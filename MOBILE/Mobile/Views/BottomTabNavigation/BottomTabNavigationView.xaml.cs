using Mobile.ViewModels;
using Mobile.ViewModels.BottomTabNavigation;
using ReactiveUI;
using ReactiveUI.XamForms;
using Sextant;
using Sextant.Abstraction;
using Splat;
using System;
using System.Reactive.Disposables;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.Xaml;

namespace Mobile.Views.BottomTabNavigation
{
    public partial class BottomTabNavigationView : ReactiveTabbedPage<BottomTabNavigationViewModel>
    {
        public BottomTabNavigationView()
        {
            InitializeComponent();

            //NavigationPage.SetHasNavigationBar(this, false);

            // To put the tab bar on bottom, Android
            On<Android>().SetToolbarPlacement(Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);

            // Enabling tab icons on uwp
            On<Windows>().EnableHeaderIcons();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                if (Children.Count == 0)
                {
                    ViewModel.TabViewModels.ForEach(x => this.Children.Add(InitNavigation(x)));
                }
            });
        }

        public NavigationPage InitNavigation(Func<IViewStackService, ITabViewModel> createViewModelFunc)
        {
            var bgScheduler = RxApp.TaskpoolScheduler;
            var mScheduler = RxApp.MainThreadScheduler;
            var vLocator = Locator.Current.GetService<IViewLocator>();

            var navigationView = new NavigationPageBase(mScheduler, bgScheduler, vLocator);
            var viewStackService = new ViewStackService(navigationView);
            var model = createViewModelFunc(viewStackService);

            navigationView.Title = model.TabTitle;
            navigationView.IconImageSource = Device.RuntimePlatform == Device.UWP ? $"Assets/{model.TabIcon}" : model.TabIcon;

            navigationView.PushPage(model as ViewModelBase, null, true, false).Subscribe();
            return navigationView;
        }
    }
}