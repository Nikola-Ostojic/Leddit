using Mobile.ViewModels.Login;
using ReactiveUI;
using System.Reactive.Disposables;

using Xamarin.Forms;

namespace Mobile.Views.Register
{
    public partial class RegisterView : ContentPageBase<RegisterViewModel>
    {
        public RegisterView()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, x => x.Email, x => x.Email.Text).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.UserName, x => x.UserName.Text).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Password, x => x.Password.Text).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.ConfirmPassword, x => x.ConfirmPassword.Text).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.Register, x => x.Password, nameof(ConfirmPassword.Completed)).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.Register, x => x.RegisterButton).DisposeWith(disposables);
            });
        }
    }
}