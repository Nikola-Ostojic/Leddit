using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using Backend.DAL.Repositories;

using System;
using System.Threading.Tasks;

using Xunit;

namespace Backend.DAL.Tests.Repositories
{
    public class UserRepositoryTests : RepositoryUnitTestsBase
    {
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests()
        {
            // Arrange
            _userRepository = new UserRepository(DbContext);
            DbContext.Users.Add(
                  new UserEntity
                  {
                      Id = 1,
                      UserName = "User",
                      Email = "user@levi9.com",
                      // Password: User
                      Password = "b512d97e7cbf97c273e4db073bbb547aa65a84589227f8f3d9e4a72b9372a24d",
                      Created = DateTime.Now,
                      Role = Role.User
                  }
                  );
            DbContext.Users.Add(
                 new UserEntity
                 {
                     Id = 2,
                     UserName = "Admin",
                     Email = "admin@levi9.com",
                     // Password: Admin
                     Password = "c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f",
                     Created = DateTime.Now,
                     Role = Role.Admin
                 });
            DbContext.SaveChanges();
        }


        [Fact]
        public async Task GetUserById_ShouldReturnUser()
        {
            // Act
            var user = await _userRepository.Get(1);

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("user@levi9.com", user.Email);
            Assert.Equal("User", user.UserName);
            Assert.Equal("User", user.Role.ToString());
        }

        [Fact]
        public async Task GetUserById_NotExistingUser_ShouldReturnNull()
        {
            // Act
            var user = await _userRepository.Get(11);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByCondition_ShouldReturnUser()
        {
            // Act
            var user = await _userRepository.FindSingleByCondition((user) => user.Email == "user@levi9.com");

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("user@levi9.com", user.Email);
            Assert.Equal("User", user.UserName);
            Assert.Equal("User", user.Role.ToString());
        }

        [Fact]
        public async Task GetUserByCondition_NotExistingUser_ShouldReturnNull()
        {
            // Act
            var user = await _userRepository.FindSingleByCondition((user) => user.Email == "us@levi9.com");

            // Assert
            Assert.Null(user);
        }
    }
}
