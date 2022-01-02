using System;
using System.Collections;
using System.Collections.Generic;

namespace Backend.DAL.Entities
{
    public class CommentEntity : BaseEntity
    {
        public string Content { get; set; }
        public UserEntity Author { get; set; }
        public ThreadEntity Thread { get; set; }
    }
}