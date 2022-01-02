using Backend.Api.DTOs;
using Backend.Api.Mappers;
using Backend.DAL.Entities;

using Xunit;

namespace Backend.Api.Tests.Mappers
{
    public class MovieMapperTests
    {
        private readonly MovieMapper _movieMapper;


        // Setup
        // The constructor is used as a setup method -> xunit
        // other testing frameworks might have specific method names
        // or annotations
        public MovieMapperTests()
        {
            _movieMapper = new MovieMapper();
        }


        [Fact]
        public void DtoToEntityTest()
        {
            // Arrange
            var dto = new MovieDTO
            {
                Id = 1,
                Name = "Kingsman the Golden Circle"
            };

            // Act
            var model = _movieMapper.ToEntity(dto);

            // Assert
            Assert.Equal(dto.Id, model.Id);
            Assert.Equal(dto.Name, model.Name);
        }

        [Fact]
        public void EntityToDtoTest()
        {
            // Arrange
            var model = new MovieEntity
            {
                Id = 1,
                Name = "The Dictator"
            };

            // Act
            var dto = _movieMapper.ToDto(model);

            // Assert
            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.Name, dto.Name);
        }
    }
}
