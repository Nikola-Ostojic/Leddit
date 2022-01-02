using Mobile.ViewModels.Movies;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;

namespace Mobile.Views.Movies
{
    public partial class MovieCellView : ReactiveViewCell<MovieCellViewModel>
    {
        public MovieCellView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Name, view => view.MovieName.Text).DisposeWith(disposables);
            });
        }
    }
}