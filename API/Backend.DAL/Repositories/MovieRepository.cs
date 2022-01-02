using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using Backend.DAL.Interfaces;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.DAL.Repositories
{
    public class MovieRepository : BaseRepository<MovieEntity>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Pageable<MovieEntity>> GetMovies(string movieName, int page, int itemsPerPage)
        {
            Expression<Func<MovieEntity, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(movieName))
            {
                filter = (movie) => movie.Name.ToLower().Contains(movieName.Trim().ToLower());
            }

            return base.FindByConditionPageable(filter, page, itemsPerPage);
        }
    }
}
