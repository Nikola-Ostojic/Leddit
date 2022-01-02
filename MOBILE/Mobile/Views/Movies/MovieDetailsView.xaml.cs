using Mobile.ViewModels.Movies;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Mobile.Views.Movies
{
    public partial class MovieDetailsView : ContentPageBase<MovieDetailsViewModel>
    {
        public MovieDetailsView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Name, view => view.MovieName.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.ImageUrl, view => view.MovieImage.Source, x => x).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.DeleteButtonVisible, view => view.DeleteMovieButton.IsVisible).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.DeleteMovie, x => x.DeleteMovieButton).DisposeWith(disposables);
            });
        }
    }
}