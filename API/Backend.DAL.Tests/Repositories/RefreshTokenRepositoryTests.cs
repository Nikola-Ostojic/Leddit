using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using Backend.DAL.Repositories;

using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.DAL.Tests.Repositories
{
    public class RefreshTokenRepositoryTests : RepositoryUnitTestsBase
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenRepositoryTests()
        {
            // Arrange
            _refreshTokenRepository = new RefreshTokenRepository(DbContext);

            var user1 = new UserEntity
            {
                UserName = "User1",
                Email = "user1@gmail.com"
            };
            var user2 = new UserEntity
            {
                UserName = "User2",
                Email = "user2@gmail.com"
            };
            DbContext.Users.Add(user1);
            DbContext.Users.Add(user2);
            DbContext.RefreshTokens.Add(new RefreshTokenEntity
            {
                Id = 1,
                Token = "token1",
                User = user1,
            });
            DbContext.RefreshTokens.Add(new RefreshTokenEntity
            {
                Id = 2,
                Token = "token2",
                User = user2
            });
            DbContext.SaveChanges();
        }

        [Fact]
        public async Task CreateRefreshToken_ShouldReturnTheCreatedRefreshToken_AndIncreaseTheRefreshTokensCount()
        {
            var refreshToken = new RefreshTokenEntity
            {
                Id = 444,
                Token = "token 123",
                User = new UserEntity
                {
                    UserName = "User1",
                    Email = "user1@gmail.com"
                }
            };
            var expectedNumberOfRefreshTokens = DbContext.RefreshTokens.Count() + 1;

            // Act
            var createdRefreshToken = await _refreshTokenRepository.Create(refreshToken);

            // Assert
            Assert.NotNull(createdRefreshToken);
            Assert.Equal(refreshToken.Token, createdRefreshToken.Token);
            Assert.Equal(refreshToken.User.UserName, createdRefreshToken.User.UserName);
            Assert.Equal(refreshToken.User.Email, createdRefreshToken.User.Email);
            Assert.True(refreshToken.Id != 0);
            Assert.Equal(expectedNumberOfRefreshTokens, DbContext.RefreshTokens.Count());
        }

        [Fact]
        public async Task IfRefreshTokenExists_UpdateRefreshToken_AndReturnUpdatedRefreshToken()
        {
            // Try to pass a new instance of the refresh token once  QueryTrackingBehavior.NoTracking is enabled
            var refreshToken = DbContext.RefreshTokens.Find(1);
            refreshToken.Token = "token112312312312";

            // Act
            var updatedRefreshToken = await _refreshTokenRepository.Update(refreshToken);

            // Assert
            Assert.NotNull(updatedRefreshToken);
            Assert.Equal(refreshToken.Id, updatedRefreshToken.Id);
            Assert.Equal(refreshToken.Token, updatedRefreshToken.Token);
            Assert.Equal(refreshToken.User.UserName, updatedRefreshToken.User.UserName);
            Assert.Equal(refreshToken.User.Email, updatedRefreshToken.User.Email);
        }

        [Fact]
        public async Task IfTheRefreshTokenToUpdate_DoesntExist_ReturnNull()
        {
            var refreshToken = new RefreshTokenEntity
            {
                Id = 4442,
                Token = "token112345",
                User = DbContext.Users.Find(1)
            };

            // Act
            var result = await _refreshTokenRepository.Update(refreshToken);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetRefreshTokenByCondition_ShouldReturnRefreshToken()
        {
            // Act
            var token = await _refreshTokenRepository.FindSingleByCondition((token) => token.Token == "token1" && token.User.UserName == "User1");

            // Assert
            Assert.Equal(1, token.Id);
            Assert.Equal("token1", token.Token);
            Assert.Equal("User1", token.User.UserName);
            Assert.Equal("user1@gmail.com", token.User.Email);
        }

        [Fact]
        public async Task GetRefreshTokenByCondition_NotExistingRefreshToken_ShouldReturnNull()
        {
            // Act
            var token = await _refreshTokenRepository.FindSingleByCondition((token) => token.Token == "tasdasdoken1" && token.User.UserName == "Useasd dadfr1");

            // Assert
            Assert.Null(token);
        }


        [Fact]
        public async Task DeleteByConditionRefreshTokenByCondition_ShouldDeleteTheRefreshTokenFromTheDatabase()
        {
            // Act
            await _refreshTokenRepository.DeleteByCondition((token) => token.Token == "token1" && token.User.UserName == "User1");

            // Assert
            Assert.Equal(1, DbContext.RefreshTokens.Count());
        }

        [Fact]
        public async Task DeleteByConditionRefreshTokenByCondition_AttemptToDeleteAnEntityThatDoesntExist_ShouldNotChangeTheTotalCount()
        {
            // Act
            await _refreshTokenRepository.DeleteByCondition((token) => token.Token == "to1231234ken1" && token.User.UserName == "Us123123er1");

            // Assert
            Assert.Equal(2, DbContext.RefreshTokens.Count());
        }

        [Fact]
        public async Task Delete_ShouldDeleteTheRefreshTokenFromTheDatabase()
        {
            // Act
            await _refreshTokenRepository.DeleteByCondition((token) => token.Token == "token1" && token.User.UserName == "User1");

            // Assert
            Assert.Equal(1, DbContext.RefreshTokens.Count());
        }

        [Fact]
        public async Task Delete_AttemptToDeleteAnEntityThatDoesntExist_ShouldNotChangeTheTotalCount()
        {
            // Act
            await _refreshTokenRepository.DeleteByCondition((token) => token.Token == "to1231234ken1" && token.User.UserName == "Us123123er1");

            // Assert
            Assert.Equal(2, DbContext.RefreshTokens.Count());
        }
    }
}
