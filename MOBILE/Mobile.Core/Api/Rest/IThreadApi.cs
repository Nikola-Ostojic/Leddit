using Mobile.Core.Dtos;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using Refit;
using System;
using System.Reactive;

namespace Mobile.Core.Api.Rest
{
    [Headers("Content-Type: application/json", "Accept: application/json")]
    public interface IThreadApi
    {
        [Get("/threads/{id}")]
        [Headers("Authorization: Bearer")]
        IObservable<ThreadResponseDTO> FetchThread(int id);

        [Get("/threads?searchCriteria={searchCriteria}&page=1&itemsPerPage={itemsPerPage}")]
        [Headers("Authorization: Bearer")]
        IObservable<PageableDTO<ThreadResponseDTO>> FetchThreads(string searchCriteria, int itemsPerPage);

        [Delete("/threads/{id}")]
        [Headers("Authorization: Bearer")]
        IObservable<string> DeleteThread(int id);

        [Post("/threads")]
        [Headers("Authorization: Bearer")]
        IObservable<Unit> CreateThread([Body] ThreadRequestDTO thread);
    }
}
