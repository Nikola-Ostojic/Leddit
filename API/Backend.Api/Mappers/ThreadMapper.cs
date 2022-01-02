using Backend.Api.DTOs.Response;
using Backend.DAL.Entities;

namespace Backend.Api.Mappers
{
    public class ThreadMapper : IMapper<ThreadEntity, ThreadResponseDTO>
    {
        public ThreadResponseDTO ToDto(ThreadEntity model)
        {
            return new ThreadResponseDTO
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content,
                Author = model.Author.UserName,
               
            };
        }

        public ThreadEntity ToEntity(ThreadResponseDTO dto)
        {
            return new ThreadEntity
            {
                Id = dto.Id,
                Title = dto.Title,
                Content = dto.Content,
            };
        }
    }
}
