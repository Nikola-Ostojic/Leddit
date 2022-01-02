using Mobile.Core.Runtime;
using Mobile.Core.Services.MoviesService;
using Mobile.Core.Services.Scheduler;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Mobile.ViewModels.Movies
{
    public class MovieDetailsViewModel : ViewModelBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IRuntimeContext _runtimeContext;

        public ReactiveCommand<Unit, Unit> GetMovie { get; protected set; }
        public ReactiveCommand<Unit, bool> DeleteMovie { get; protected set; }

        private int _id;
        public new int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => this.RaiseAndSetIfChanged(ref _imageUrl, value);
        }

        private bool _deleteButtonVisible;
        public bool DeleteButtonVisible
        {
            get => _deleteButtonVisible;
            set => this.RaiseAndSetIfChanged(ref _deleteButtonVisible, value);
        }

        public MovieDetailsViewModel(
            int id,
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IMoviesService moviesService = null,
            IRuntimeContext runtimeContext = null) : base(schedulerService, viewStackService)
        {
            _moviesService = moviesService ?? Locator.Current.GetService<IMoviesService>();
            _runtimeContext = runtimeContext ?? Locator.Current.GetService<IRuntimeContext>();

            GetMovie = ReactiveCommand.CreateFromObservable(() =>
                 _moviesService.GetMovie(id),
                 outputScheduler: _schedulerService.TaskPoolScheduler);

            DeleteMovie = ReactiveCommand.CreateFromObservable(() => _moviesService.Delete(id), outputScheduler: _schedulerService.TaskPoolScheduler);

            DeleteButtonVisible = _runtimeContext.Role == "Admin";

            DeleteMovie
                .ObserveOn(_schedulerService.MainScheduler)
                .SubscribeOn(_schedulerService.MainScheduler)
                .Subscribe(x => _viewStackService.PopPage().Subscribe());

            _moviesService
                .Movie
                .Where(m => m != null)
                .SubscribeOn(_schedulerService.TaskPoolScheduler)
                .ObserveOn(_schedulerService.MainScheduler)
                .Subscribe(m =>
                {
                    Id = m.Id;
                    Name = m.Name;
                    ImageUrl = m.ImageUrl;
                });

            Observable
              .Merge(
                  GetMovie.ThrownExceptions,
                  DeleteMovie.ThrownExceptions)
              .ObserveOn(_schedulerService.MainScheduler)
              .SelectMany(ex => ShowGenericError(string.Empty, ex))
              .Subscribe();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                Observable.Return(Unit.Default).InvokeCommand(this, x => x.GetMovie);
            });
        }
    }
}
