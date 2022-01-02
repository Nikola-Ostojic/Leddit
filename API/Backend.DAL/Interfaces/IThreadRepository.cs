using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using System.Threading.Tasks;

namespace Backend.DAL.Interfaces
{
    public interface IThreadRepository
    {
        Task<ThreadEntity> Get(int id);
        Task<ThreadEntity> GetWithoutNavigationProperties(int id);
        Task<Pageable<ThreadEntity>> GetThreads(string searchCriteria, int page, int itemsPerPage);
        Task<ThreadEntity> Create(ThreadEntity thread);
        Task<ThreadEntity> Update(ThreadEntity thread);
        Task Delete(int id);
    }
}
