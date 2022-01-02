using Backend.Api.DTOs.Request;
using Backend.Api.DTOs.Response;
using Backend.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Backend.Api.Mappers
{
    public class CommentMapper : IMapper<CommentEntity, CommentResponseDTO>
    {
        public CommentResponseDTO ToDto(CommentEntity model)
        {
            return new CommentResponseDTO
            {
                Id = model.Id,
                Content = model.Content,
                Author = model.Author.UserName
            };
        }

        public CommentEntity ToEntity(CommentResponseDTO dto)
        {
            return new CommentEntity
            {
                Id = dto.Id,
                Content = dto.Content
                            };
        }
    }
}
