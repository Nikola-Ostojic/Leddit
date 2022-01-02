using DynamicData;
using Mobile.Core.Api;
using Mobile.Core.Api.Rest;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using Mobile.Core.Extensions;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Mobile.Core.Services.ThreadService
{
    public class ThreadService : IThreadService
    {
        private readonly IApiService<IThreadApi> _threadApi;

        private readonly Subject<ThreadResponseDTO> _thread = new Subject<ThreadResponseDTO>();
        public IObservable<ThreadResponseDTO> Thread => _thread.AsObservable();

        private readonly SourceCache<ThreadResponseDTO, int> _threads = new SourceCache<ThreadResponseDTO, int>(x => x.Id);
        public IObservableCache<ThreadResponseDTO, int> Threads => _threads;

        public ThreadService(IApiService<IThreadApi> threadApi = null)
        {
            _threadApi = threadApi ?? Locator.Current.GetService<IApiService<IThreadApi>>();
        }

        public IObservable<Unit> GetThread(int id) =>
            _threadApi.GetClient().FetchThread(id).Select(thread =>
            {
                _thread.OnNext(thread);
                return Unit.Default;
            });

        public IObservable<Unit> GetThreads(string searchCriteria, int itemsPerPage) =>
            _threadApi.GetClient().FetchThreads(searchCriteria, itemsPerPage).Select(result =>
            {
                _threads.EditDiff(result.Data);
                return Unit.Default;
            });

        public IObservable<bool> Delete(int id)
        {
            return _threadApi.GetClient().DeleteThread(id).Select(x => true);
        }

        public IObservable<Unit> Create(ThreadRequestDTO thread)
        {
            return _threadApi.GetClient().CreateThread(thread);
        }
    }
}
