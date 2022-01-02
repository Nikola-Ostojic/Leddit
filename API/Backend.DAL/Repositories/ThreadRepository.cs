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
    public class ThreadRepository : BaseRepository<ThreadEntity>, IThreadRepository
    {
        private readonly ICommentRepository _commentRepository;
        public ThreadRepository(ApplicationDbContext context, ICommentRepository commentRepository) : base(context)
        {
            _commentRepository = commentRepository;
        }

        public Task<Pageable<ThreadEntity>> GetThreads(string searchCriteria, int page, int itemsPerPage)
        {
            Func<IQueryable<ThreadEntity>, IQueryable<ThreadEntity>> inclusion = (thread) => thread.Include(t => t.Author);

            Func<IQueryable<ThreadEntity>, IQueryable<ThreadEntity>> sorting = (thread) => thread.OrderByDescending(t => t.Created);

            Expression<Func<ThreadEntity, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(searchCriteria))
            {
                filter = (thread) => thread.Title.ToLower().Contains(searchCriteria.Trim().ToLower())
                || thread.Content.ToLower().Contains(searchCriteria.Trim().ToLower());
            }

            var retVal = base.FindByConditionPageable(filter, page, itemsPerPage, inclusion, sorting);
            return retVal;
        }

        public override Task<ThreadEntity> Get(int id)
        {
            return _entities.AsNoTracking().Include(t => t.Author).FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task Delete(int id)
        {
            await _commentRepository.DeleteCommentForThread(id);
            await base.Delete(id);
        }

        public Task<ThreadEntity> GetWithoutNavigationProperties(int id)
        {
            return _entities.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
