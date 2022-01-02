using Backend.Core.Interfaces;
using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<UserEntity> Create(UserEntity userEntity)
        {
            return _userRepository.Create(userEntity);
        }

        public Task<UserEntity> GetUser(string email, string password)
        {
            Expression<Func<UserEntity, bool>> condition = (user) => user.Email == email && user.Password == password;

            return _userRepository.FindSingleByCondition(condition);
        }

        public Task<UserEntity> GetUserByEmail(string email)
        {
            return _userRepository.FindSingleByCondition((user) => user.Email == email);
        }
    }
}
