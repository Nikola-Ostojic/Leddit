using System.Collections.Generic;

namespace Backend.DAL.Entities
{
    public class ThreadEntity : BaseEntity 
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public UserEntity Author { get; set; }
        public List<CommentEntity> Comments { get; set; }
    }
}