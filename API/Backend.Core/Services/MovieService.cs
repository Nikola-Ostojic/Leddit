using Backend.Core.Interfaces;
using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using Backend.DAL.Interfaces;

using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _moviesRepository;
        public MovieService(IMovieRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }

        public Task<MovieEntity> Get(int id)
        {
            return _moviesRepository.Get(id);
        }

        public Task<Pageable<MovieEntity>> GetMovies(string movieName, int page, int itemsPerPage)
        {
            return _moviesRepository.GetMovies(movieName, page, itemsPerPage);
        }

        public Task<MovieEntity> Create(MovieEntity movie)
        {
            return _moviesRepository.Create(movie);
        }

        public Task<MovieEntity> Update(MovieEntity movie)
        {
            return _moviesRepository.Update(movie);
        }

        public Task Delete(int id)
        {
            return _moviesRepository.Delete(id);
        }
    }
}
