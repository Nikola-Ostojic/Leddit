using Backend.Api.DTOs;
using Backend.DAL.Helpers;
using System.Linq;

namespace Backend.Api.Mappers
{
    /// <summary>
    /// Generic Mapper interface for us to abide when implementing our own mappers.
    /// There are other solutions as well, for example, AutoMapper.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public interface IMapper<TEntity, TDto>
    {
        TEntity ToEntity(TDto dto);
        TDto ToDto(TEntity model);

        // Default implementation
        PageableDTO<TDto> ToPageableDto(Pageable<TEntity> pageableEntity)
        {
            return new PageableDTO<TDto>
            {
                Data = pageableEntity.Data.Select(ToDto).ToList(),
                Page = pageableEntity.Page,
                TotalItems = pageableEntity.TotalItems,
                TotalPages = pageableEntity.TotalPages
            };
        }
    }
}
