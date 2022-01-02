using Mobile.ViewModels.Thread;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Mobile.Views.Threads
{
    public partial class ThreadDetailsView : ContentPageBase<ThreadDetailsViewModel>
    {
        public ThreadDetailsView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Title, view => view.Title.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Content, view => view.Content.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Author, view => view.Author.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.CommentsCount, view => view.CommentsCount.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Comments, view => view.CommentsList.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.ItemsPerPage, view => view.CommentsList.PageSize).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.GetComments, view => view.CommentsList.LoadMoreCommand).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.CommentText, view => view.CommentTextField.Text).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.AddComment, x => x.AddCommentButton).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.DeleteButtonVisible, view => view.DeleteThreadButton.IsVisible).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.DeleteThread, x => x.DeleteThreadButton).DisposeWith(disposables);

                Observable
              .Return(ViewModel.ItemsPerPage, RxApp.TaskpoolScheduler)
              .InvokeCommand(ViewModel, x => x.GetComments);
            });
        }
    }
}