using System.ComponentModel.DataAnnotations;

namespace Jarboo.Admin.Web.Models.Account
{
    public class LandingViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [EmailAddress]
        [Compare("Email", ErrorMessage = "The email and confirmation do not match.")]
        public string ConfirmEmail { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}