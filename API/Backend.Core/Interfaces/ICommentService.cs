using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface ICommentService
    {
        Task<CommentEntity> Get(int id);
        Task<int> GetCommentsCount(int threadId);
        Task<Pageable<CommentEntity>> GetComments(int threadId, int page, int itemsPerPage);
        Task<CommentEntity> Create(CommentEntity comment);
        Task<CommentEntity> Update(CommentEntity comment);
        Task Delete(int id);
    }
}
