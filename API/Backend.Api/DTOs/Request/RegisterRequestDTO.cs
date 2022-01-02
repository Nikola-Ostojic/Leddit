using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.Request
{
    public class RegisterRequestDTO : IValidatableObject
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Customize the rule set for registration
            // For example alphanumerical characters in passwords...
            // Minimum length
            // As a rule of thumb it is good to have this validation in the service it self as well.
            // For example, right now our service can only be consumed through rest endpoint and in that case this is ok.
            // But if we were to create a class library as a utility package for other applications to use, the "users" might not validate
            // the register object beforehand, therefore, the "AuthService" should handle that part as well
            if (string.IsNullOrWhiteSpace(UserName))
                yield return new ValidationResult("Must provide UserName.");
            if (string.IsNullOrWhiteSpace(Email))
                yield return new ValidationResult("Must provide Email.");
            if (!new EmailAddressAttribute().IsValid(Email))
                yield return new ValidationResult("Must provide a valid email.");
            if (string.IsNullOrWhiteSpace(Password))
                yield return new ValidationResult("Must provide Password.");
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
                yield return new ValidationResult("Must provide ConfirmPassword.");
            if (Password != ConfirmPassword)
                yield return new ValidationResult("Password and confirm password must match.");
        }
    }
}
