using Mobile.ViewModels.Thread;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Views.Threads
{
    public partial class ThreadView : ContentPageBase<ThreadViewModel>
    {
        public ThreadView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Threads, view => view.ThreadList.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedThread, view => view.ThreadList.SelectedItem).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SearchCriteria, view => view.SearchCriteria.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.CurrentPageSize, view => view.ThreadList.PageSize).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ClickAddThread, x => x.AddNewThreadButton).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.IsRunning, view => view.ActivityIndicator.IsLoading).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.GetThreads, view => view.ThreadList.LoadMoreCommand).DisposeWith(disposables);

                Observable
                  .Return(ViewModel.CurrentPageSize, RxApp.TaskpoolScheduler)
                  .InvokeCommand(ViewModel, x => x.GetThreads);
            });
        }
    }
}