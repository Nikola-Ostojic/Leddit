using Mobile.Core.Dtos.Request;
using Mobile.Core.Services.MoviesService;
using Mobile.Core.Services.Scheduler;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.Movies
{
    public class AddMovieViewModel : ViewModelBase
    {
        private readonly IMoviesService _moviesService;

        public ReactiveCommand<Unit, Unit> AddMovie { get; protected set; }

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

        public AddMovieViewModel(
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IMoviesService moviesService = null) : base(schedulerService, viewStackService)
        {
            _moviesService = moviesService ?? Locator.Current.GetService<IMoviesService>();

            var canAdd = this
               .WhenAnyValue(x => x.Name, x => x.ImageUrl,
                   (_name, _imageUrl) => (!string.IsNullOrWhiteSpace(_name) && !string.IsNullOrWhiteSpace(_imageUrl)));

            AddMovie = ReactiveCommand.CreateFromObservable(() =>
                _moviesService.Create(new CreateMovieRequestDTO
                {
                    Name = Name,
                    ImageUrl = ImageUrl
                }),
                canAdd,
                outputScheduler: _schedulerService.TaskPoolScheduler
            );

            var addMoviePublish = AddMovie
                .Publish();

            addMoviePublish
                .ObserveOn(_schedulerService.MainScheduler)
                .Subscribe(_ => ShowSuccessMessage("Movie created!").Subscribe());

            addMoviePublish
                .ObserveOn(_schedulerService.MainScheduler)
                .Select(_ => _viewStackService.PopPage())
                .Subscribe();

            addMoviePublish.Connect();

            AddMovie
               .ThrownExceptions
               .SubscribeOn(_schedulerService.TaskPoolScheduler)
               .ObserveOn(_schedulerService.MainScheduler)
               .SelectMany(ex => this.ShowGenericError("", ex))
               .Subscribe();
        }
    }
}