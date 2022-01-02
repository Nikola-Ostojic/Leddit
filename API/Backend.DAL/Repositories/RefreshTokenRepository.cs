using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.DAL.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshTokenEntity>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override Task<RefreshTokenEntity> FindSingleByCondition(Expression<Func<RefreshTokenEntity, bool>> condition)
        {
            // Forcing eager loading on foreign tables
            _entities.Include(r => r.User).Load();
            return base.FindSingleByCondition(condition);
        }
    }
}
