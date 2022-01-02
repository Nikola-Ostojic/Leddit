using System.Collections.Generic;

namespace Backend.DAL.Helpers
{
    public class Pageable<T>
    {
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; }
    }
}
