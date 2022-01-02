using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.Request
{
    public class TokenRequestDTO : IValidatableObject
    {
        public string RefreshToken { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(RefreshToken))
                yield return new ValidationResult("Must provide RefreshToken.");
        }
    }
}
