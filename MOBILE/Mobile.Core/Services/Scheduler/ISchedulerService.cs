using System.Reactive.Concurrency;

namespace Mobile.Core.Services.Scheduler
{
    public interface ISchedulerService
    {
        IScheduler DefaultScheduler { get; }

        IScheduler CurrentThreadScheduler { get; }

        IScheduler ImmediateScheduler { get; }

        IScheduler MainScheduler { get; }

        IScheduler TaskPoolScheduler { get; }
    }
}
