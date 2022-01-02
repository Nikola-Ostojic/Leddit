using Backend.DAL.Entities;

using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity> GetUser(string email, string password);
        Task<UserEntity> Create(UserEntity userEntity);
        Task<UserEntity> GetUserByEmail(string email);
    }
}
