using Mobile.ViewModels.Thread;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Mobile.Views.Threads
{
    public partial class AddThreadView : ContentPageBase<AddThreadViewModel>
    {
        public AddThreadView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.Title, view => view.ThreadTitle.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Content, view => view.ThreadContent.Text).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.AddThread, x => x.AddThreadButton).DisposeWith(disposables);
            });
        }
    }
}