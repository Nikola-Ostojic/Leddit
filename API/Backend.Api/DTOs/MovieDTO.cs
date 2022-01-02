using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs
{
    public class MovieDTO : IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult("Must provide the movie Name.");
        }
    }
}
