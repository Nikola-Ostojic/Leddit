using Backend.Core.Interfaces;
using Backend.DAL.Entities;
using Backend.DAL.Interfaces;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }


        public Task<RefreshTokenEntity> Create(string token, DateTime validFor, UserEntity user)
        {
            return _refreshTokenRepository.Create(new RefreshTokenEntity
            {
                Token = token,
                User = user,
                ValidFor = validFor,
            });
        }

        public Task<RefreshTokenEntity> GetToken(string refreshToken)
        {
            Expression<Func<RefreshTokenEntity, bool>> condition = (token) => token.Token == refreshToken;

            return _refreshTokenRepository.FindSingleByCondition(condition);
        }

        public Task RevokeToken(string email)
        {
            Expression<Func<RefreshTokenEntity, bool>> tokensFilter = (token) => token.User.Email == email;

            return _refreshTokenRepository.DeleteByCondition(tokensFilter);
        }

        public Task RevokeToken(int tokenId)
        {
            return _refreshTokenRepository.Delete(tokenId);
        }

        public Task<RefreshTokenEntity> Update(RefreshTokenEntity refreshToken)
        {
            return _refreshTokenRepository.Update(refreshToken);
        }
    }
}
