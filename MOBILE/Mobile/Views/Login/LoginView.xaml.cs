using Mobile.ViewModels.Login;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Mobile.Views.Login
{
    public partial class LoginView : ContentPageBase<LoginViewModel>
    {
        public LoginView()
        {
            //NavigationPage.SetHasNavigationBar(this, true);
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, x => x.Email, x => x.Email.Text).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Password, x => x.Password.Text).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.Login, x => x.Password, nameof(Password.Completed)).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.Login, x => x.LoginButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.NavigateToRegistrationPage, x => x.NavigateToRegistration).DisposeWith(disposables);

            });
        }
    }
}