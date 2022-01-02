using Mobile.ViewModels.Thread;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;

namespace Mobile.Views.Threads
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThreadCellView : ReactiveViewCell<ThreadCellViewModel>
    {
        public ThreadCellView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Title, view => view.Title.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Content, view => view.Content.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Author, view => view.Author.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.CommentsCount, view => view.CommentsCount.Text).DisposeWith(disposables);
            });
        }
    }
}