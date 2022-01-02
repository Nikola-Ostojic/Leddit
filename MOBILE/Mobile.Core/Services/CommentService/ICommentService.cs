using DynamicData;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using System;
using System.Reactive;

namespace Mobile.Core.Services.CommentService
{
    public interface ICommentService
    {
        IObservable<CommentResponseDTO> Comment { get; }
        IObservableCache<CommentResponseDTO, int> Comments { get; }
        IObservable<Unit> GetComments(int threadId, int itemsPerPage);
        IObservable<Unit> GetComment(int id);
        IObservable<Unit> Create(CommentRequestDTO comment);
        IObservable<bool> Delete(int id);
    }
}
