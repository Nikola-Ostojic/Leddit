using Mobile.Core.Extensions;

namespace Mobile.Core.Dtos.Response
{
    public class CommentResponseDTO : ModelWithIdBase<int>
    {
        public string Content { get; set; }

        public string Author { get; set; }
    }
}
