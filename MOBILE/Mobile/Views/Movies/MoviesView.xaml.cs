using Mobile.ViewModels.Movies;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace Mobile.Views.Movies
{
    public partial class MoviesView : ContentPageBase<MoviesViewModel>
    {
        public MoviesView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Movies, view => view.MoviesList.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedMovie, view => view.MoviesList.SelectedItem).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.MovieName, view => view.MovieName.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.CurrentPageSize, view => view.MoviesList.PageSize).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ClickAddMovie, x => x.AddNewMovieButton).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.ClickAddButtonVisible, view => view.AddNewMovieButton.IsEnabled).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.IsRunning, view => view.ActivityIndicator.IsLoading).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.GetMovies, view => view.MoviesList.LoadMoreCommand).DisposeWith(disposables);

                Observable
                  .Return(ViewModel.CurrentPageSize, RxApp.TaskpoolScheduler)
                  .InvokeCommand(ViewModel, x => x.GetMovies);
            });
        }
    }
}