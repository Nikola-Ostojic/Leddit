using Mobile.ViewModels.Profile;
using ReactiveUI;
using System.Reactive.Disposables;
using Xamarin.Forms;

namespace Mobile.Views.Profile
{
    public partial class ProfileView : ContentPageBase<ProfileViewModel>
    {
        public ProfileView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.UserName, view => view.UserNameLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Email, view => view.EmailLabel.Text, x => x).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Logout, view => view.LogoutButton).DisposeWith(disposables);
            });
        }
    }
}