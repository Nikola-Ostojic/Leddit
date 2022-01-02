using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using Backend.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.DAL.Repositories
{
    public class CommentRepository : BaseRepository<CommentEntity>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Pageable<CommentEntity>> GetComments(int threadId, int page, int itemsPerPage)
        {
            Func<IQueryable<CommentEntity>, IQueryable<CommentEntity>> inclusion = (comment) => comment.Include(t => t.Author).Include(c => c.Thread);
            Func<IQueryable<CommentEntity>, IQueryable<CommentEntity>> sorting = (coment) => coment.OrderByDescending(t => t.Created);

            Expression<Func<CommentEntity, bool>> filter = null;
            if (threadId != 0)
            {
                filter = (comment) => comment.Thread.Id == threadId;
            }

            var retVal = base.FindByConditionPageable(filter, page, itemsPerPage, inclusion, sorting);
            return retVal;
        }

        public override Task<CommentEntity> Get(int threadId)
        {
            return _entities.AsNoTracking()
                .Include(c => c.Author)
                .FirstOrDefaultAsync(s => s.Id == threadId);
        }

        public Task DeleteCommentForThread(int id)
        {
            Expression<Func<CommentEntity, bool>> filter = (comment) => comment.Thread.Id == id;

            return DeleteByCondition(filter);
        }

        public Task<int> GetCommentsCount(int threadId)
        {
            return _entities.Where(c => c.Thread.Id == threadId).CountAsync();
        }
    }
}
