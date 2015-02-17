using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Models
{
    public class CustomerCreate : IValidatableObject
    {
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Creator { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [MinLength(8)]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Email)) ||
                (string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password)))
            {
                yield return new ValidationResult("All user's properties must be provided");
            }
        }
    }
}
