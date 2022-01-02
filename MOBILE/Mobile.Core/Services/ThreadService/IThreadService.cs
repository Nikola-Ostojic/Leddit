using DynamicData;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using System;
using System.Reactive;

namespace Mobile.Core.Services.ThreadService
{
    public interface IThreadService
    {
        IObservable<ThreadResponseDTO> Thread { get; }
        IObservableCache<ThreadResponseDTO, int> Threads { get; }
        IObservable<Unit> GetThreads(string searchCriteria, int itemsPerPage);
        IObservable<Unit> GetThread(int id);
        IObservable<Unit> Create(ThreadRequestDTO thread);
        IObservable<bool> Delete(int id);
    }
}
