using Mobile.Core.Runtime;
using Mobile.Core.Services.Scheduler;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using System.Reactive;
using System.Reactive.Linq;

namespace Mobile.ViewModels.UnitTests
{
    public abstract class ViewModelTestBase
    {
        protected TestSchedulerService _schedulerService;
        protected Mock<IViewStackService> _viewStackService;
        protected Mock<IRuntimeContext> _runtimeContextMock;

        [SetUp]
        protected virtual void Setup()
        {
            Splat.ModeDetector.InUnitTestRunner();

            _schedulerService = new TestSchedulerService();
            RxApp.MainThreadScheduler = _schedulerService.MainScheduler;
            RxApp.TaskpoolScheduler = _schedulerService.MainScheduler;

            _viewStackService = new Mock<IViewStackService>() { DefaultValue = DefaultValue.Mock };
            _viewStackService
                .Setup(x => x.PushPage(It.IsAny<IPageViewModel>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(() => Observable.Return(Unit.Default));
            _viewStackService
                .Setup(x => x.PushModal(It.IsAny<IPageViewModel>(), It.IsAny<string>()))
                .Returns(() => Observable.Return(Unit.Default));
            _viewStackService
                .Setup(x => x.PopPage(It.IsAny<bool>()))
                .Returns(() => Observable.Return(Unit.Default));
            _viewStackService
                .Setup(x => x.PopModal(It.IsAny<bool>()))
                .Returns(() => Observable.Return(Unit.Default));
            _viewStackService
                .Setup(x => x.PopToRootPage(It.IsAny<bool>()))
                .Returns(() => Observable.Return(Unit.Default));
            _runtimeContextMock = new Mock<IRuntimeContext>() { DefaultValue = DefaultValue.Mock };

            RegisterDependencies();
        }

        protected virtual void RegisterDependencies()
        {
            Locator.CurrentMutable.Register(() => _schedulerService, typeof(ISchedulerService));
            Locator.CurrentMutable.Register(() => _viewStackService.Object, typeof(IViewStackService));
            Locator.CurrentMutable.Register(() => _runtimeContextMock.Object, typeof(IRuntimeContext));
        }
    }
}
