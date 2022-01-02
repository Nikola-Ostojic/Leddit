using Backend.Api.DTOs.Response;
using Backend.Api.Mappers;
using Backend.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Backend.Api.Tests.Mappers
{
    public class CommentMapperTests
    {
        private readonly CommentMapper _commentMapper;


        // Setup
        // The constructor is used as a setup method -> xunit
        // other testing frameworks might have specific method names
        // or annotations
        public CommentMapperTests()
        {
            _commentMapper = new CommentMapper();
        }


        [Fact]
        public void DtoToEntityTest()
        {
            // Arrange
            var dto = new CommentResponseDTO
            {
                Id = 1,
                Content = "Sunny day."
            };

            // Act
            var model = _commentMapper.ToEntity(dto);

            // Assert
            Assert.Equal(dto.Id, model.Id);
            Assert.Equal(dto.Content, model.Content);
        }

        [Fact]
        public void EntityToDtoTest()
        {
            // Arrange
            var model = new CommentEntity
            {
                Id = 1,
                Content = "Rainy day.",
                Author = new UserEntity
                {
                    UserName = "Test authorr"
                }
            };

            // Act
            var dto = _commentMapper.ToDto(model);

            // Assert
            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.Content, dto.Content);
            Assert.Equal(model.Author.UserName, dto.Author);
        }
    }
}
