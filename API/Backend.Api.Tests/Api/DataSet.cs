using Backend.DAL.Entities;
using System.Collections.Generic;

namespace Backend.Api.Tests.Api
{
    public class DataSet
    {
        public List<ThreadEntity> Threads { get; set; }
        public List<MovieEntity> Movies { get; set; }

        public List<CommentEntity> Comments { get; set; }

        public List<UserEntity> Users { get; set; }
        public List<RefreshTokenEntity> RefreshTokens { get; set; }
    }
}
