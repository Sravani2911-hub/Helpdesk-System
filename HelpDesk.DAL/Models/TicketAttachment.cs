using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.DAL.Models
{
    public class TicketAttachment
    {
        [Key]//this tells EF Core: This is the Primary Key
        public int AttachmentId { get; set; }

        [Required]//FileName cannot be empty
        [StringLength(255)]//Maximum length:255 characters
        public string FileName { get; set; } = string.Empty; //eg:ErrorScreenshot.png

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedDate { get; set; } = DateTime.Now;

        // Foreign Key - Ticket
        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket? Ticket { get; set; }

        // Foreign Key - User
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}