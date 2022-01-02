using Mobile.Core.Dtos;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using Refit;
using System;
using System.Reactive;

namespace Mobile.Core.Api.Rest
{
    [Headers("Content-Type: application/json", "Accept: application/json")]
    public interface ICommentApi
    {
        [Get("/comments/{id}")]
        [Headers("Authorization: Bearer")]
        IObservable<CommentResponseDTO> FetchComment(int id);

        [Get("/comments?threadId={threadId}&page=1&itemsPerPage={itemsPerPage}")]
        [Headers("Authorization: Bearer")]
        IObservable<PageableDTO<CommentResponseDTO>> FetchComments(int threadId, int itemsPerPage);

        [Delete("/comments/{id}")]
        [Headers("Authorization: Bearer")]
        IObservable<string> DeleteComment(int id);

        [Post("/comments")]
        [Headers("Authorization: Bearer")]
        IObservable<Unit> CreateComment([Body] CommentRequestDTO comment);
    }
}
