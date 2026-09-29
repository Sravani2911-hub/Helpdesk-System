using System.ComponentModel.DataAnnotations;

namespace HelpDesk.DAL.Models
{
    public class Category
    {
        [Key] //this tells EF Core: This is the Primary Key.
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required")] //Category Name cannot be empty
        [StringLength(100,ErrorMessage = "Maximum 100 characters allowed")] //Maximum length:100 characters
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; } 
    }
}