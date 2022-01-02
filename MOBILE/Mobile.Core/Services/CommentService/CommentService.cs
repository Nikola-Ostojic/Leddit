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

namespace Mobile.Core.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly IApiService<ICommentApi> _commentApi;

        private readonly Subject<CommentResponseDTO> _comment = new Subject<CommentResponseDTO>();
        public IObservable<CommentResponseDTO> Comment => _comment.AsObservable();

        private readonly SourceCache<CommentResponseDTO, int> _comments = new SourceCache<CommentResponseDTO, int>(x => x.Id);
        public IObservableCache<CommentResponseDTO, int> Comments => _comments;

        public CommentService(IApiService<ICommentApi> commentApi = null)
        {
            _commentApi = commentApi ?? Locator.Current.GetService<IApiService<ICommentApi>>();
        }

        public IObservable<Unit> GetComment(int id) =>
            _commentApi.GetClient().FetchComment(id).Select(comment =>
            {
                _comment.OnNext(comment);
                return Unit.Default;
            });

        public IObservable<Unit> GetComments(int threadId, int itemsPerPage) =>
            _commentApi.GetClient().FetchComments(threadId, itemsPerPage).Select(result =>
            {
                _comments.EditDiff(result.Data);
                return Unit.Default;
            });

        public IObservable<bool> Delete(int id)
        {
            return _commentApi.GetClient().DeleteComment(id).Select(x => true);
        }

        public IObservable<Unit> Create(CommentRequestDTO comment)
        {
            return _commentApi.GetClient().CreateComment(comment);
        }
    }
}
