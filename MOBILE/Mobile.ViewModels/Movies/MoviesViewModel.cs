using DynamicData;
using Mobile.Core.Runtime;
using Mobile.Core.Services.MoviesService;
using Mobile.Core.Services.Scheduler;
using Mobile.ViewModels.BottomTabNavigation;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.Movies
{
    public class MoviesViewModel : ViewModelBase, ITabViewModel
    {
        private readonly IMoviesService _moviesService;
        private readonly IRuntimeContext _runtimeContext;

        public ReactiveCommand<Unit, Unit> ClickAddMovie { get; protected set; }

        public string TabTitle => "Movies";
        public string TabIcon => "TabMovies.png";

        private string movieName = string.Empty;
        public string MovieName
        {
            get => movieName;
            set => this.RaiseAndSetIfChanged(ref movieName, value);
        }

        private int currentPageSize = 25;
        public int CurrentPageSize
        {
            get => currentPageSize;
            set => this.RaiseAndSetIfChanged(ref currentPageSize, value);
        }

        private bool _clickAddButtonVisible;
        public bool ClickAddButtonVisible
        {
            get => _clickAddButtonVisible;
            set => this.RaiseAndSetIfChanged(ref _clickAddButtonVisible, value);
        }


        private readonly ReadOnlyObservableCollection<MovieCellViewModel> _movies;
        public ReadOnlyObservableCollection<MovieCellViewModel> Movies => _movies;
        private MovieCellViewModel _selectedMovie;
        public MovieCellViewModel SelectedMovie
        {
            get => _selectedMovie;
            set => this.RaiseAndSetIfChanged(ref _selectedMovie, value);
        }

        public ReactiveCommand<int, Unit> GetMovies { get; protected set; }

        public MoviesViewModel(
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IMoviesService moviesService = null,
            IRuntimeContext runtimeContext = null) : base(schedulerService, viewStackService)
        {
            _moviesService = moviesService ?? Locator.Current.GetService<IMoviesService>();
            _runtimeContext = runtimeContext ?? Locator.Current.GetService<IRuntimeContext>();

            ClickAddButtonVisible = _runtimeContext.Role == "Admin";

            GetMovies = ReactiveCommand.CreateFromObservable<int, Unit>((itemsPerPage) =>
            _moviesService.GetMovies(MovieName, itemsPerPage),
                outputScheduler: _schedulerService.TaskPoolScheduler);

            ClickAddMovie = ReactiveCommand.CreateFromObservable(() => _viewStackService.PushPage(new AddMovieViewModel()));

            // Connecting the dynamic data source cache with the view model's list
            // Changes are displayed on the UI immediately after the service cache gets updated
            _moviesService
                .Movies
                .Connect()
                .Transform(x => new MovieCellViewModel(x.Id, x.Name, x.ImageUrl))
                .ObserveOn(_schedulerService.MainScheduler)
                .Bind(out _movies)
                .DisposeMany()
                .Subscribe();

            // Handling the flag for activity indicator when the command is executing
            GetMovies
                .IsExecuting
                .SubscribeOn(_schedulerService.TaskPoolScheduler)
                .Select(x => IsRunning = x);

            // When any of the Reactive Commands throw an error handle it here
            Observable
             .Merge(
                 GetMovies.ThrownExceptions,
                 ClickAddMovie.ThrownExceptions)
             .ObserveOn(_schedulerService.MainScheduler)
             .SelectMany(ex => ShowGenericError(string.Empty, ex))
             .Subscribe();

            // When a movie gets selected navigate to the details page
            this.WhenAnyValue(x => x.SelectedMovie)
                    .Where(post => post != null)
                    .ObserveOn(_schedulerService.MainScheduler)
                    .SelectMany(vm => _viewStackService.PushPage(new MovieDetailsViewModel(vm.Id, viewStackService: _viewStackService)))
                    .Subscribe();

            // When movie name changes trigger get movies command with the name
            // as the filtering parameter
            this.WhenAnyValue(x => x.MovieName)
                .Throttle(TimeSpan.FromSeconds(1))
                .ObserveOn(_schedulerService.MainScheduler)
                .Select((name) => Observable.Return(25).InvokeCommand(this, x => x.GetMovies))
                .Subscribe();

        }

    }
}
