using Backend.DAL.Entities;
using Backend.DAL.Helpers;

using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IMovieService
    {
        Task<MovieEntity> Get(int id);
        Task<Pageable<MovieEntity>> GetMovies(string movieName, int page, int itemsPerPage);
        Task<MovieEntity> Create(MovieEntity movie);
        Task<MovieEntity> Update(MovieEntity movie);
        Task Delete(int id);
    }
}
