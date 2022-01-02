using Backend.Core.Interfaces;
using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using Backend.DAL.Interfaces;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public Task<CommentEntity> Get(int id)
        {
            return _commentRepository.Get(id);
        }

        public Task<Pageable<CommentEntity>> GetComments(int threadId, int page, int itemsPerPage)
        {
            return _commentRepository.GetComments(threadId, page, itemsPerPage);
        }

        public Task<int> GetCommentsCount(int threadId)
        {
            return _commentRepository.GetCommentsCount(threadId);
        }

        public Task<CommentEntity> Create(CommentEntity comment)
        {
            return _commentRepository.Create(comment);
        }

        public Task<CommentEntity> Update(CommentEntity comment)
        {
            return _commentRepository.Update(comment);
        }

        public Task Delete(int id)
        {
            return _commentRepository.Delete(id);
        }
    }
}
