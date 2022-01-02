using DynamicData;
using Mobile.Core.Services.Scheduler;
using Mobile.Core.Services.ThreadService;
using Mobile.ViewModels.BottomTabNavigation;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Mobile.ViewModels.Thread
{
    public class ThreadViewModel : ViewModelBase, ITabViewModel
    {
        private readonly IThreadService _threadService;

        public ReactiveCommand<Unit, Unit> ClickAddThread { get; protected set; }

        public string TabTitle => "Threads";
        public string TabIcon => "TabHome.png";

        private string searchCriteria = string.Empty;
        public string SearchCriteria
        {
            get => searchCriteria;
            set => this.RaiseAndSetIfChanged(ref searchCriteria, value);
        }

        private int currentPageSize = 25;
        public int CurrentPageSize
        {
            get => currentPageSize;
            set => this.RaiseAndSetIfChanged(ref currentPageSize, value);
        }

        private readonly ReadOnlyObservableCollection<ThreadCellViewModel> _threads;
        public ReadOnlyObservableCollection<ThreadCellViewModel> Threads => _threads;
        private ThreadCellViewModel _selectedThread;
        public ThreadCellViewModel SelectedThread
        {
            get => _selectedThread;
            set => this.RaiseAndSetIfChanged(ref _selectedThread, value);
        }

        public ReactiveCommand<int, Unit> GetThreads { get; protected set; }

        public ThreadViewModel(
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IThreadService threadService = null) : base(schedulerService, viewStackService)
        {
            _threadService = threadService ?? Locator.Current.GetService<IThreadService>();

            GetThreads = ReactiveCommand.CreateFromObservable<int, Unit>((itemsPerPage) =>
            _threadService.GetThreads(SearchCriteria, itemsPerPage),
                outputScheduler: _schedulerService.TaskPoolScheduler);

            ClickAddThread = ReactiveCommand.CreateFromObservable(() => _viewStackService.PushPage(new AddThreadViewModel(viewStackService: _viewStackService)));

            // Connecting the dynamic data source cache with the view model's list
            // Changes are displayed on the UI immediately after the service cache gets updated
            _threadService
                .Threads
                .Connect()
                .Transform(x => new ThreadCellViewModel(x.Id, x.Title, x.Content, x.Author, x.CreatedAt, x.ModifiedAt, x.CommentsCount))
                .ObserveOn(_schedulerService.MainScheduler)
                .Bind(out _threads)
                .DisposeMany()
                .Subscribe();

            // Handling the flag for activity indicator when the command is executing
            GetThreads
                .IsExecuting
                .SubscribeOn(_schedulerService.TaskPoolScheduler)
                .Select(x => IsRunning = x);

            // When any of the Reactive Commands throw an error handle it here
            Observable
             .Merge(
                 GetThreads.ThrownExceptions,
                 ClickAddThread.ThrownExceptions)
             .ObserveOn(_schedulerService.MainScheduler)
             .SelectMany(ex => ShowGenericError(string.Empty, ex))
             .Subscribe();

            // Exception
            GetThreads
                .ThrownExceptions
                .SelectMany(ex => ShowGenericError(ex.Message, ex))
                .Subscribe();

            // When a thread gets selected navigate to the details page
            this.WhenAnyValue(x => x.SelectedThread)
                    .Where(post => post != null)
                    .ObserveOn(_schedulerService.MainScheduler)
                    .SelectMany(vm => _viewStackService.PushPage(new ThreadDetailsViewModel(vm.Id, viewStackService: _viewStackService)))
                    .Subscribe();

            // When thread search criteria changes trigger get thread command with the search criteria
            // as the filtering parameter
            this.WhenAnyValue(x => x.SearchCriteria)
                .Throttle(TimeSpan.FromSeconds(1))
                .ObserveOn(_schedulerService.MainScheduler)
                .Select((name) => Observable.Return(25).InvokeCommand(this, x => x.GetThreads))
                .Subscribe();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                SelectedThread = null;
            });
        }
    }
}
