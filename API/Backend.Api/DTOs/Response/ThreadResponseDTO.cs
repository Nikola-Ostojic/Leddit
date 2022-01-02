using System;

namespace Backend.Api.DTOs.Response
{
    public class ThreadResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int CommentsCount { get; set; }
    }
}
