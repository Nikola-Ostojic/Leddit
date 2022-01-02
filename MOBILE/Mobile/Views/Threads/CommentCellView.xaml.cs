using Mobile.ViewModels.Thread;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;

namespace Mobile.Views.Threads
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentCellView : ReactiveViewCell<CommentCellViewModel>
    {
        public CommentCellView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Author, view => view.Author.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Content, view => view.Content.Text).DisposeWith(disposables);
            });
        }
    }
}