using System.ComponentModel.DataAnnotations;

namespace HelpDesk.DAL.Models
{
    public class Department
    {
        [Key]  //this tells EF Core: This is the Primary Key.
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department Name is required")] //Department Name cannot be empty
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")] //Maximum length:100 characters
        public string DepartmentName { get; set; } = string.Empty; 

        public bool IsActive { get; set; } = true;
    }
}