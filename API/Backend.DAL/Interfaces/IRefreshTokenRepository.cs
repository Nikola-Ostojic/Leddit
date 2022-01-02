using Backend.DAL.Entities;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.DAL.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenEntity> Create(RefreshTokenEntity refreshToken);
        Task<RefreshTokenEntity> Update(RefreshTokenEntity refreshToken);
        Task<RefreshTokenEntity> FindSingleByCondition(Expression<Func<RefreshTokenEntity, bool>> condition);
        Task DeleteByCondition(Expression<Func<RefreshTokenEntity, bool>> tokensFilter);
        Task Delete(int id);
    }
}
