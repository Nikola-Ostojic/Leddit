using Mobile.Core.Extensions;

namespace Mobile.Core.Dtos
{
    public class MovieDTO : ModelWithIdBase<int>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
