using System.ComponentModel.DataAnnotations;

namespace HelpDesk.DAL.Models
{
    public class Priority
    {
        [Key]//this tells EF Core: This is the Primary Key.
        public int PriorityId { get; set; }

        [Required(ErrorMessage = "Priority Name is required")] //Priority Name cannot be empty
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]//Maximum length:100 characters
        public string PriorityName { get; set; } = string.Empty;

        public int ResolutionHours { get; set; } //why ResolutionHours : This will help us implement SLA (Service Level Agreement) later.

        public bool IsActive { get; set; } 
    }
}