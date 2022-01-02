using Backend.Api.DTOs;
using Backend.Api.Mappers;
using Backend.Core.Interfaces;
using Backend.DAL.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _moviesService;
        private readonly ILogger<MoviesController> _logger;
        private readonly IMapper<MovieEntity, MovieDTO> _movieMapper;

        public MoviesController(ILogger<MoviesController> logger, IMovieService moviesService, IMapper<MovieEntity, MovieDTO> movieMapper)
        {
            _logger = logger;
            _moviesService = moviesService;
            _movieMapper = movieMapper;
        }

        [HttpGet]
        public async Task<ActionResult<PageableDTO<MovieDTO>>> GetMovies(string movieName, int page, int itemsPerPage)
        {
            _logger.LogInformation("GetMovies");
            var result = await _moviesService.GetMovies(movieName, page, itemsPerPage);
            var mappedResult = _movieMapper.ToPageableDto(result);
            return mappedResult;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await _moviesService.Get(id);
            if (movie == null)
            {
                return NotFound($"Movie with the id of {id} doesn't exist");
            }

            return _movieMapper.ToDto(movie);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MovieDTO>> Create([FromBody] MovieDTO movie)
        {
            var createdMovie = await _moviesService.Create(_movieMapper.ToEntity(movie));

            return Created($"movies/{createdMovie.Id}", _movieMapper.ToDto(createdMovie));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MovieDTO>> Update(int id, [FromBody] MovieDTO movie)
        {
            var updatedMovie = await _moviesService.Update(_movieMapper.ToEntity(movie));
            return _movieMapper.ToDto(updatedMovie);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var movieToDelete = await _moviesService.Get(id);
            if (movieToDelete == null)
            {
                return NotFound($"Couldn't find a movie with the id of {id} to delete");
            }

            await _moviesService.Delete(id);
            return NoContent();
        }
    }
}
