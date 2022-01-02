using Mobile.ViewModels.Movies;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Mobile.Views.Movies
{
    public partial class AddMovieView : ContentPageBase<AddMovieViewModel>
    {
        public AddMovieView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.Name, view => view.MovieName.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.ImageUrl, view => view.MovieImageUrl.Text).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.AddMovie, x => x.AddMovieButton).DisposeWith(disposables);
            });
        }
    }
}