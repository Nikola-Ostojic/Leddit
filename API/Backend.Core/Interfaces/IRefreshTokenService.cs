using Backend.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IRefreshTokenService
    {
        Task RevokeToken(string email);
        Task RevokeToken(int tokenId);
        Task<RefreshTokenEntity> Create(string token, DateTime validFor, UserEntity user);
        Task<RefreshTokenEntity> GetToken(string refreshToken);
        Task<RefreshTokenEntity> Update(RefreshTokenEntity refreshToken);
    }
}
