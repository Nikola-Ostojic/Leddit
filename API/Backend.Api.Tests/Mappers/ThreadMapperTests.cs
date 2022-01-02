using Backend.Api.DTOs;
using Backend.Api.DTOs.Response;
using Backend.Api.Mappers;
using Backend.DAL.Entities;

using Xunit;

namespace Backend.Api.Tests.Mappers
{
    public class ThreadMapperTests
    {
        private readonly ThreadMapper _threadMapper;

        public ThreadMapperTests()
        {
            _threadMapper = new ThreadMapper();
        }

        [Fact]
        public void DtoToEntityTest()
        {
            // Arrange
            var dto = new ThreadResponseDTO
            {
                Id = 1,
                Title = "New movie in cinemas : Kingsman the Golden Circle"
            };

            // Act
            var model = _threadMapper.ToEntity(dto);

            // Assert
            Assert.Equal(dto.Id, model.Id);
            Assert.Equal(dto.Title, model.Title);
        }

        [Fact]
        public void EntityToDtoTest()
        {
            // Arrange
            var model = new ThreadEntity
            {
                Id = 1,
                Title = "No new movies this month",
                Author = new UserEntity
                {
                    UserName = "Test author"
                }
            };

            // Act
            var dto = _threadMapper.ToDto(model);

            // Assert
            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.Title, dto.Title);
            Assert.Equal(model.Author.UserName, dto.Author);

        }
    }
}
