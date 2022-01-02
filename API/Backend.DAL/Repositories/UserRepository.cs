using Backend.DAL.Entities;
using Backend.DAL.Interfaces;

namespace Backend.DAL.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
