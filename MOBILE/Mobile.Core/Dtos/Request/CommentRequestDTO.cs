namespace Mobile.Core.Dtos.Request
{
    public class CommentRequestDTO
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string Content { get; set; }
    }
}
