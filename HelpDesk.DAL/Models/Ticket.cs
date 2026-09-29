
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.DAL.Models
{
    public class Ticket
    {
        [Key]//this tells EF Core: This is the Primary Key.
        public int TicketId { get; set; }

        
        [StringLength(20)]//Maximum length:20 characters
        public string TicketNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        // Foreign Key - Department
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        // Foreign Key - Category
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")] //This tells EF Core:The Category object is related using the CategoryId column
        public Category? Category { get; set; } //This is called a Navigation Property.(Instead of writing SQL joins manually, EF Core lets us navigate between related objects)

        // Foreign Key - Priority
        public int PriorityId { get; set; }

        [ForeignKey("PriorityId")]
        public Priority? Priority { get; set; }


        // Employee who created the ticket
        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public ApplicationUser? CreatedBy { get; set; }

        // Support Engineer assigned to the ticket
        public string? AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public ApplicationUser? AssignedTo { get; set; }

        public string? AttachmentPath { get; set; }
        
        [NotMapped]
        public IFormFile? AttachmentFile { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Open";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<TicketComment>? Comments { get; set; }

        public ICollection<TicketAttachment>? Attachments { get; set; }

        public ICollection<TicketHistory>? TicketHistories { get; set; }

      
    }
}