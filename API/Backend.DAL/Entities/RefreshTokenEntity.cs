using System;

namespace Backend.DAL.Entities
{
    public class RefreshTokenEntity : BaseEntity
    {
        public string Token { get; set; }
        public UserEntity User { get; set; }
        public DateTime ValidFor { get; set; }
    }
}
