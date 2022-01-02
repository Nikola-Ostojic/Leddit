using Mobile.ViewModels.Login;
using Mobile.Views;
using ReactiveUI;
using Sextant;
using Sextant.Abstraction;
using Splat;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var bootstrapper = new Bootstrapper();
            bootstrapper.RegisterServices();
            bootstrapper.RegisterViews();

            MainPage = Initialise();
        }

        private NavigationPage Initialise()
        {
            var bgScheduler = RxApp.TaskpoolScheduler;
            var mScheduler = RxApp.MainThreadScheduler;
            var vLocator = Locator.Current.GetService<IViewLocator>();

            var navigationView = new NavigationPageBase(mScheduler, bgScheduler, vLocator);
            var viewStackService = new ViewStackService(navigationView);

            Locator.CurrentMutable.Register<IViewStackService>(() => viewStackService);
            navigationView.PushPage(new AutoLoginViewModel(), null, true, false).Subscribe();

            return navigationView;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
