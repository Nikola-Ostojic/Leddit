using Backend.Core.Interfaces;
using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using Backend.DAL.Interfaces;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IThreadRepository _threadsRepository;
        public ThreadService(IThreadRepository threadsRepository)
        {
            _threadsRepository = threadsRepository;
        }

        public Task<ThreadEntity> Get(int id)
        {
            return _threadsRepository.Get(id);
        }

        public Task<Pageable<ThreadEntity>> GetThreads(string searchCriteria, int page, int itemsPerPage)
        {
            return _threadsRepository.GetThreads(searchCriteria, page, itemsPerPage);
        }

        public Task<ThreadEntity> Create(ThreadEntity thread)
        {
            return _threadsRepository.Create(thread);
        }

        public Task<ThreadEntity> Update(ThreadEntity thread)
        {
            return _threadsRepository.Update(thread);
        }

        public Task Delete(int id)
        {
            return _threadsRepository.Delete(id);
        }

        public Task<ThreadEntity> GetWithoutUser(int id)
        {
            return _threadsRepository.GetWithoutNavigationProperties(id);
        }
    }
}
