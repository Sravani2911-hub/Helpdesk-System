using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Presentation.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")] //User's name
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]  //Used as the username
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)] //User's password
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)] //Ensures both passwords match
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        //[Required]
        //[Display(Name = "Role")]
        //public string Role { get; set; } = "Employee";
    }
}