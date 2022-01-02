using System.Collections.Generic;

namespace Mobile.Core.Dtos
{
    public class PageableDTO<T>
    {
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; }
    }
}
