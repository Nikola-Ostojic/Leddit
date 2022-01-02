using Backend.Api.DTOs;
using Backend.DAL.Entities;

namespace Backend.Api.Mappers
{
    public class MovieMapper : IMapper<MovieEntity, MovieDTO>
    {
        public MovieDTO ToDto(MovieEntity model)
        {
            return new MovieDTO
            {
                Id = model.Id,
                Name = model.Name,
                ImageUrl = model.ImageUrl
            };
        }

        public MovieEntity ToEntity(MovieDTO dto)
        {
            return new MovieEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                ImageUrl = dto.ImageUrl
            };
        }
    }
}
