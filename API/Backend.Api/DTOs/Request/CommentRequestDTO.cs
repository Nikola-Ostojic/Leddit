using Backend.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.DTOs.Request
{
    public class CommentRequestDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int ThreadId { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Content))
                yield return new ValidationResult("Must provide the Content.");
        }
    }
}
