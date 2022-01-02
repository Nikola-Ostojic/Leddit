using Mobile.Core.Extensions;
using System;

namespace Mobile.Core.Dtos.Response
{
    public class ThreadResponseDTO : ModelWithIdBase<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int CommentsCount { get; set; }
    }
}
