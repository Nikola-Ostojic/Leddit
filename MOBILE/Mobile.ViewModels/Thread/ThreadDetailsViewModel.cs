using DynamicData;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Runtime;
using Mobile.Core.Services.CommentService;
using Mobile.Core.Services.Scheduler;
using Mobile.Core.Services.ThreadService;
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
    public class ThreadDetailsViewModel : ViewModelBase
    {
        private readonly IThreadService _threadService;
        private readonly ICommentService _commentService;
        private readonly IRuntimeContext _runtimeContext;

        public ReactiveCommand<Unit, Unit> AddComment { get; protected set; }

        public ReactiveCommand<Unit, bool> DeleteThread { get; protected set; }

        public ReactiveCommand<Unit, Unit> GetThread { get; protected set; }

        public ReactiveCommand<int, Unit> GetComments { get; protected set; }

        private int _id;
        public new int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

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

        private int _commentsCount;
        public int CommentsCount
        {
            get => _commentsCount;
            set => this.RaiseAndSetIfChanged(ref _commentsCount, value);
        }

        private string _author;
        public string Author
        {
            get => _author;
            set => this.RaiseAndSetIfChanged(ref _author, value);
        }

        private int _itemsPerPage = 25;
        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set => this.RaiseAndSetIfChanged(ref _itemsPerPage, value);
        }

        private string _commentText;
        public string CommentText
        {
            get => _commentText;
            set => this.RaiseAndSetIfChanged(ref _commentText, value);
        }

        private readonly ReadOnlyObservableCollection<CommentCellViewModel> _comments;
        public ReadOnlyObservableCollection<CommentCellViewModel> Comments => _comments;

        private bool _deleteButtonVisible;
        public bool DeleteButtonVisible
        {
            get => _deleteButtonVisible;
            set => this.RaiseAndSetIfChanged(ref _deleteButtonVisible, value);
        }

        public ThreadDetailsViewModel(
            int id,
            ISchedulerService schedulerService = null,
            IViewStackService viewStackService = null,
            IThreadService threadService = null,
            ICommentService commentService = null,
            IRuntimeContext runtimeContext = null) : base(schedulerService, viewStackService)
        {
            _threadService = threadService ?? Locator.Current.GetService<IThreadService>();
            _commentService = commentService ?? Locator.Current.GetService<ICommentService>();
            _runtimeContext = runtimeContext ?? Locator.Current.GetService<IRuntimeContext>();

            //Thread
            GetThread = ReactiveCommand.CreateFromObservable(() =>
                 _threadService.GetThread(id),
                 outputScheduler: _schedulerService.TaskPoolScheduler);

            //Comment
            GetComments = ReactiveCommand.CreateFromObservable<int, Unit>(itemsPerPage =>
                _commentService.GetComments(id, itemsPerPage),
                outputScheduler: _schedulerService.TaskPoolScheduler);

            // Create comment
            var canAdd = this
             .WhenAnyValue(x => x.CommentText,
                 (_commentText) => (!string.IsNullOrWhiteSpace(_commentText)));

            AddComment = ReactiveCommand.CreateFromObservable(() =>
                _commentService.Create(new CommentRequestDTO
                {
                    ThreadId = Id,
                    Content = CommentText
                }),
                canAdd,
                outputScheduler: _schedulerService.MainScheduler
            );

            var addCommentPublish = AddComment.Publish();

            addCommentPublish
                .ObserveOn(_schedulerService.MainScheduler)
                .Subscribe(_ => ShowSuccessMessage("Comment created!").Subscribe());

            addCommentPublish
               .ObserveOn(_schedulerService.MainScheduler)
               .Subscribe(_ => Observable.Return(ItemsPerPage).InvokeCommand(GetComments));

            addCommentPublish
            .ObserveOn(_schedulerService.MainScheduler)
            .Subscribe(_ => Observable.Return(Unit.Default).InvokeCommand(this, x => x.GetThread));

            addCommentPublish
              .ObserveOn(_schedulerService.MainScheduler)
              .Subscribe(_ => CommentText = "");

            addCommentPublish.Connect();

            _threadService
                .Thread
                .Where(m => m != null)
                .SubscribeOn(_schedulerService.TaskPoolScheduler)
                .ObserveOn(_schedulerService.MainScheduler)
                .Subscribe(m =>
                {
                    Id = m.Id;
                    Title = m.Title;
                    Content = m.Content;
                    Author = m.Author;
                    CommentsCount = m.CommentsCount;
                    DeleteButtonVisible = _runtimeContext.Role == "Admin" || _runtimeContext.UserName == m.Author;
                });

            _commentService
              .Comments
              .Connect()
              .Transform(x => new CommentCellViewModel(x.Id, x.Author, x.Content))
              .ObserveOn(_schedulerService.MainScheduler)
              .Bind(out _comments)
              .DisposeMany()
              .Subscribe();

            Observable
              .Merge(
                  GetThread.ThrownExceptions,
                  GetComments.ThrownExceptions)
              .ObserveOn(_schedulerService.MainScheduler)
              .SelectMany(ex => ShowGenericError(string.Empty, ex))
              .Subscribe();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                Observable.Return(Unit.Default).InvokeCommand(this, x => x.GetThread);
            });

            AddComment
               .ThrownExceptions
               .SubscribeOn(_schedulerService.TaskPoolScheduler)
               .ObserveOn(_schedulerService.MainScheduler)
               .SelectMany(ex => this.ShowGenericError("", ex))
               .Subscribe();
            
            DeleteThread = ReactiveCommand.CreateFromObservable(() => _threadService.Delete(id), outputScheduler: _schedulerService.TaskPoolScheduler);
            
            DeleteThread
              .ObserveOn(_schedulerService.MainScheduler)
              .SubscribeOn(_schedulerService.MainScheduler)
              .Subscribe(x => _viewStackService.PopPage().Subscribe());
            
            Observable
            .Merge(
                GetThread.ThrownExceptions,
                DeleteThread.ThrownExceptions)
            .ObserveOn(_schedulerService.MainScheduler)
            .SelectMany(ex => ShowGenericError(string.Empty, ex))
            .Subscribe();
        }
    }
}
