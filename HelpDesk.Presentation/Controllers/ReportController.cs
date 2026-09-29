using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.BAL.Interfaces;
using HelpDesk.Presentation.Models;
using ClosedXML.Excel;
using System.IO;

namespace HelpDesk.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly ITicketService _ticketService;

        public ReportController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        //Gets all tickets from the database. counts total tickets,open tickets ,in progress tickets etc..
        public async Task<IActionResult> Index(DateTime? fromDate,DateTime? toDate,string? status)
        {
            var tickets = await _ticketService.GetAllTicketsAsync();

            if (fromDate.HasValue)
            {
                tickets = tickets.Where(t => t.CreatedDate.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                tickets = tickets.Where(t => t.CreatedDate.Date <= toDate.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                tickets = tickets.Where(t => t.Status == status);
            }

            ReportViewModel model = new()
            {
                TotalTickets = tickets.Count(),

                OpenTickets = tickets.Count(t => t.Status == "Open"),

                InProgressTickets = tickets.Count(t => t.Status == "In Progress"),

                ResolvedTickets = tickets.Count(t => t.Status == "Resolved"),

                ClosedTickets = tickets.Count(t => t.Status == "Closed"),

                FromDate = fromDate,
                ToDate = toDate, 
                Status = status,

                Tickets = tickets
            };

            return View(model);
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Tickets");

            worksheet.Cell(1, 1).Value = "Ticket Number";
            worksheet.Cell(1, 2).Value = "Title";
            worksheet.Cell(1, 3).Value = "Department";
            worksheet.Cell(1, 4).Value = "Priority";
            worksheet.Cell(1, 5).Value = "Status";
            worksheet.Cell(1, 6).Value = "Created Date";

            int row = 2;

            foreach (var ticket in tickets)
            {
                worksheet.Cell(row, 1).Value = ticket.TicketNumber;
                worksheet.Cell(row, 2).Value = ticket.Title;
                worksheet.Cell(row, 3).Value = ticket.Department?.DepartmentName;
                worksheet.Cell(row, 4).Value = ticket.Priority?.PriorityName;
                worksheet.Cell(row, 5).Value = ticket.Status;
                worksheet.Cell(row, 6).Value =
                    ticket.CreatedDate.ToString("dd-MMM-yyyy");

                row++;
            }

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "HelpDesk_Report.xlsx");
        }   

    }
}