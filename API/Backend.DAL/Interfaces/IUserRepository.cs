using Backend.DAL.Entities;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> Get(int id);
        Task<UserEntity> Create(UserEntity user);
        Task<UserEntity> FindSingleByCondition(Expression<Func<UserEntity, bool>> condition);
    }
}
