using Mobile.Core.Services.Scheduler;
using Mobile.Infrastructure.Exceptions;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System;
using System.Reactive;

namespace Mobile.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IPageViewModel, ISupportsActivation
    {
        protected readonly ISchedulerService _schedulerService;
        protected readonly IViewStackService _viewStackService;

        // For UWP the title needs to be at least an empty space, or the navigation bar wont be displayed
        private string _id = " ";
        public virtual string Id => _id;

        private bool _isRunning;

        public bool IsRunning
        {
            get => _isRunning;
            set => this.RaiseAndSetIfChanged(ref _isRunning, value);
        }

        protected readonly ViewModelActivator _viewModelActivator = new ViewModelActivator();
        public ViewModelActivator Activator => _viewModelActivator;

        protected readonly Interaction<string, Unit> _showAlert;
        public Interaction<string, Unit> ShowAlert => _showAlert;

        protected readonly Interaction<(string, string), Unit> _showInfo;
        public Interaction<(string title, string message), Unit> ShowInfo => _showInfo;

        protected readonly Interaction<string, Unit> _showError;
        public Interaction<string, Unit> ShowError => _showError;

        protected readonly Interaction<string, Unit> _showSuccess;
        public Interaction<string, Unit> ShowSuccess => _showSuccess;

        protected readonly Interaction<string, bool> _confirm;
        public Interaction<string, bool> Confirm => _confirm;

        public ViewModelBase(
            ISchedulerService schedulerService,
            IViewStackService viewStackService,
            string title = " ")
        {
            _viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();
            _schedulerService = schedulerService ?? Locator.Current.GetService<ISchedulerService>();

            _id = title;
            _showAlert = new Interaction<string, Unit>(_schedulerService.MainScheduler);
            _showError = new Interaction<string, Unit>(_schedulerService.MainScheduler);
            _showInfo = new Interaction<(string title, string message), Unit>(_schedulerService.MainScheduler);
            _showSuccess = new Interaction<string, Unit>(_schedulerService.MainScheduler);
            _confirm = new Interaction<string, bool>(_schedulerService.MainScheduler);
        }

        public IObservable<Unit> ShowGenericError(string errorMessage = "", Exception exception = null)
        {
            var message = GetErrorMessageText(errorMessage, exception);

            return ShowError.Handle(message);
        }

        public IObservable<Unit> ShowInfoMessage(string title, string message) =>
            ShowInfo.Handle((title, message));

        public IObservable<Unit> ShowSuccessMessage(string successMessage = "") =>
            ShowSuccess.Handle(string.IsNullOrEmpty(successMessage) ? "Success" : successMessage);

        public IObservable<bool> ShowConfirmation(string message) =>
            Confirm.Handle(message);

        public IObservable<Unit> ShowAlertMessage(string message) => ShowAlert.Handle(message);

        private string GetErrorMessageText(string errorMessage = "", Exception exception = null)
        {
            return string.IsNullOrEmpty(errorMessage)
                ? (exception != null && (exception is GenericException || exception is NoInternetConnectionException) ? exception.Message : "An error happened")
                : errorMessage;
        }
    }
}
