using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.Request
{
    public class LoginRequestDTO : IValidatableObject
    {
        public string Password { get; set; }
        public string Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Email))
                yield return new ValidationResult("Must provide Email.");
            if (string.IsNullOrWhiteSpace(Password))
                yield return new ValidationResult("Must provide Password.");
        }
    }
}
