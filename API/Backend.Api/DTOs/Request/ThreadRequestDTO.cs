using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.Request
{
    public class ThreadRequestDTO : IValidatableObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Title))
                yield return new ValidationResult("Must provide the thread Title.");
            else if (string.IsNullOrWhiteSpace(Content))
                yield return new ValidationResult("Must provide the thread Content.");
        }
    }
}
