using Mobile.Core.Dtos.Request;
using Mobile.Core.Services.Scheduler;
using Mobile.Core.Services.ThreadService;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.Thread
{
    public class AddThreadViewModel : ViewModelBase
    {
        private readonly IThreadService _threadsService;

        public ReactiveCommand<Unit, Unit> AddThread { get; protected set; }

        private string _title;
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public AddThreadViewModel(
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IThreadService threadsService = null) : base(schedulerService, viewStackService)
        {
            _threadsService = threadsService ?? Locator.Current.GetService<IThreadService>();

            var canAdd = this
               .WhenAnyValue(x => x.Title, x => x.Content,
                   (_title, _content) => (!string.IsNullOrWhiteSpace(_title) && !string.IsNullOrWhiteSpace(_content)));

            AddThread = ReactiveCommand.CreateFromObservable(() =>
                _threadsService.Create(new ThreadRequestDTO
                {
                    Title = Title,
                    Content = Content
                }),
                canAdd,
                outputScheduler: _schedulerService.TaskPoolScheduler
            );

            var addThreadPublish = AddThread
                .Publish();

            addThreadPublish
                .ObserveOn(_schedulerService.MainScheduler)
                .Subscribe(_ => ShowSuccessMessage("Thread created!").Subscribe());

            addThreadPublish
                .ObserveOn(_schedulerService.MainScheduler)
                .Select(_ => _viewStackService.PopPage(true))
                .Subscribe();

            addThreadPublish.Connect();

            AddThread
               .ThrownExceptions
               .SubscribeOn(_schedulerService.TaskPoolScheduler)
               .ObserveOn(_schedulerService.MainScheduler)
               .SelectMany(ex => this.ShowGenericError("", ex))
               .Subscribe();
        }
    }
}
