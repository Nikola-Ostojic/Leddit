using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using System.Threading.Tasks;

namespace Backend.DAL.Interfaces
{
    public interface ICommentRepository
    {
        Task<CommentEntity> Get(int id);
        Task<Pageable<CommentEntity>> GetComments(int threadId, int page, int itemsPerPage);
        Task<CommentEntity> Create(CommentEntity comment);
        Task<CommentEntity> Update(CommentEntity comment);
        Task Delete(int id);
        Task DeleteCommentForThread(int id);
        Task<int> GetCommentsCount(int threadId);
    }
}
