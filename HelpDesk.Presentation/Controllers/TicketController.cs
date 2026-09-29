using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Models;
using HelpDesk.Presentation.Services;
using HelpDesk.Presentation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Security.Claims;

namespace HelpDesk.Presentation.Controllers
{

    public class TicketController : Controller //inject
    {
        private readonly ITicketService _ticketService;
        private readonly IDepartmentService _departmentService;
        private readonly ICategoryService _categoryService;
        private readonly IPriorityService _priorityService;
        private readonly ITicketCommentService _ticketCommentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;
        private readonly ITicketHistoryService _ticketHistoryService;
        private readonly INotificationService _notificationService;


        public TicketController(
            ITicketService ticketService,
            IDepartmentService departmentService,
            ICategoryService categoryService,
            IPriorityService priorityService,
            ITicketCommentService ticketCommentService,
               UserManager<ApplicationUser> userManager,
               IWebHostEnvironment webHostEnvironment,
                  IEmailService emailService,
                   ITicketHistoryService ticketHistoryService,
                   INotificationService notificationService)
        {
            _ticketService = ticketService;
            _departmentService = departmentService;
            _categoryService = categoryService;
            _priorityService = priorityService;
            _ticketCommentService = ticketCommentService;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
            _ticketHistoryService = ticketHistoryService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index(string searchText)
        {
            IEnumerable<Ticket> tickets;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                tickets = await _ticketService.SearchTicketsAsync(searchText);
            }
            else
            {
                tickets = await _ticketService.GetAllTicketsAsync();
            }

            ViewBag.SearchText = searchText;

            return View(tickets);
        }

        [Authorize(Roles = "Support Engineer")]
        public async Task<IActionResult> MyAssignedTickets()
        {
            // Get the currently logged-in user's Id
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get only the tickets assigned to this user
            var tickets = await _ticketService.GetAssignedTicketsAsync(userId);

            return View(tickets);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> MyTickets()
        {
            // Get logged-in employee UserId
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get only tickets created by this employee
            var tickets = await _ticketService.GetMyTicketsAsync(userId);

            return View(tickets);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            var comments = await _ticketCommentService.GetCommentsByTicketIdAsync(id);

            var history = await _ticketHistoryService.GetHistoryByTicketIdAsync(id);

            var model = new TicketDetailsViewModel
            {
                Ticket = ticket,
                Comments = comments,
                History = history
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int ticketId, string newComment)
        {
            if (!string.IsNullOrWhiteSpace(newComment))
            {
                TicketComment comment = new TicketComment
                {
                    TicketId = ticketId,
                    UserId = _userManager.GetUserId(User),
                    CommentText = newComment,
                    CommentDate = DateTime.Now
                };

                await _ticketCommentService.AddCommentAsync(comment);

                await _ticketHistoryService.AddHistoryAsync(new TicketHistory
                {
                    TicketId = ticketId,
                    UserId = _userManager.GetUserId(User),
                    Action = "Added Comment",
                    ActionDate = DateTime.Now
                });
            }

            return RedirectToAction(nameof(Details), new { id = ticketId });
        }

        //Create Ticket Page
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {

            // Temporary Ticket Number
            ticket.TicketNumber = "INC-" + DateTime.Now.Ticks.ToString().Substring(10); // //Generate Ticket Number

            ticket.Status = "Open"; //Every new ticket starts in the Open state.

            ticket.CreatedDate = DateTime.Now; //The system automatically stores the creation date and time.

            ticket.CreatedById = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {

                if (ticket.AttachmentFile == null)
                {
                    TempData["Error"] = "AttachmentFile is NULL";
                }
                else
                {
                    TempData["Error"] =
                        $"File Name: {ticket.AttachmentFile.FileName} | Size: {ticket.AttachmentFile.Length}";

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Guid.NewGuid().ToString() +
                                      Path.GetExtension(ticket.AttachmentFile.FileName);

                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ticket.AttachmentFile.CopyToAsync(stream);
                    }

                    ticket.AttachmentPath = "/uploads/" + fileName;
                }

                //{
                //    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                //    if (!Directory.Exists(uploadsFolder))
                //    {
                //        Directory.CreateDirectory(uploadsFolder);
                //    }

                //    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ticket.AttachmentFile.FileName);

                //    string filePath = Path.Combine(uploadsFolder, fileName);

                //    using (var stream = new FileStream(filePath, FileMode.Create))
                //    {
                //        await ticket.AttachmentFile.CopyToAsync(stream);
                //    }

                //    ticket.AttachmentPath = "/uploads/" + fileName;
                //}
                await _ticketService.AddTicketAsync(ticket);

                await _notificationService.AddNotificationAsync(new Notification
                {
                    UserId = ticket.CreatedById!,
                    TicketId = ticket.TicketId,
                    Message = $"Your ticket '{ticket.Title}' has been created successfully.",
                    CreatedDate = DateTime.Now,
                    IsRead = false
                });

                await _ticketHistoryService.AddHistoryAsync(new TicketHistory
                {
                    TicketId = ticket.TicketId,
                    UserId = _userManager.GetUserId(User),
                    Action = "Ticket Created",
                    ActionDate = DateTime.Now
                });

                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    if (user != null)
                    {
                        string subject = "HelpDesk - Ticket Created Successfully";

                        string body =
$@"Hello {user.FullName},

Your support ticket has been created successfully.

Employee Name : {user.FullName}
Employee Email: {user.Email}

Ticket Number : {ticket.TicketNumber}
Title         : {ticket.Title}
Status        : {ticket.Status}

Thank you,
HelpDesk Team";

                        await _emailService.SendEmailAsync(user.Email!, subject, body);

                        return RedirectToAction(nameof(Index));
                    }

                    return Content("User is null.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ticket created, but email could not be sent.");

                    await LoadDropdowns();

                    return View(ticket);
                }
            }

                await LoadDropdowns();

            return View(ticket);
        }

        // GET: Edit Ticket
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            // 🔒 Nobody can edit a closed ticket
            if (ticket.Status == "Closed")
            {
                TempData["Error"] = "🔒 This ticket is closed and cannot be edited.";

                return RedirectToAction(nameof(Details), new { id = ticket.TicketId });
            }

            // Employee can edit only when ticket is Open
            if (User.IsInRole("Employee") && ticket.Status != "Open")
            {
                TempData["Error"] = "You cannot edit this ticket because work has already started.";
                return RedirectToAction(nameof(Index));
            }

            await LoadDropdowns();

            var engineers = await _userManager.GetUsersInRoleAsync("Support Engineer");

            ViewBag.SupportEngineers = new SelectList(
                engineers,
                "Id",
                "FullName",
                ticket.AssignedToId);

            return View(ticket);
        }

        //Update Ticket
        [HttpPost]  //Runs when the Update Ticket button is clicked.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ticket ticket)
        {
            if (ModelState.IsValid) //Checks if all required fields are valid.
            {
                var existingTicket = await _ticketService.GetTicketByIdAsync(ticket.TicketId);

                if (existingTicket != null && existingTicket.Status == "Closed")
                {
                    TempData["Error"] = "Closed tickets cannot be edited.";

                    return RedirectToAction(nameof(Details), new { id = ticket.TicketId });
                }

                await _ticketService.UpdateTicketAsync(ticket); //Sends the updated ticket to the service layer.

                return RedirectToAction(nameof(Index)); //Returns to the Ticket List after saving
            }

            await LoadDropdowns();

            var engineers = await _userManager.GetUsersInRoleAsync("Support Engineer");

            ViewBag.SupportEngineers = new SelectList(
                engineers,
                "Id",
                "FullName",
                ticket.AssignedToId);

            return View(ticket);
        }


        // GET: Delete Ticket
        public async Task<IActionResult> Delete(int id)  //Gets the ticket by ID.
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);  //If the ticket doesn't exist, returns 404 Not Found.

            if (ticket == null)  //Otherwise, opens the Delete Confirmation page
            {
                return NotFound();
            }

            return View(ticket);
        }
        // POST: Delete Ticket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Ticket ticket)
        {
            await _ticketService.DeleteTicketAsync(ticket.TicketId);

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            ViewBag.Departments = new SelectList(
                await _departmentService.GetAllDepartmentsAsync(),
                "DepartmentId",
                "DepartmentName");

            ViewBag.Categories = new SelectList(
                await _categoryService.GetAllCategoriesAsync(),
                "CategoryId",
                "CategoryName");

            ViewBag.Priorities = new SelectList(
                await _priorityService.GetAllPrioritiesAsync(),
                "PriorityId",
                "PriorityName");

            var supportEngineers = await _userManager.GetUsersInRoleAsync("Support Engineer");

            ViewBag.SupportEngineers = new SelectList(
                supportEngineers,
                "Id",
                "FullName");
        }

        [HttpGet]
        [Authorize(Roles = "Support Engineer")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Support Engineer")]
        public async Task<IActionResult> UpdateStatus(Ticket ticket)
        {
            TempData["Success"] = "POST UpdateStatus Action Hit";

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine($"{error.Key} : {e.ErrorMessage}");
                    }
                }

                return View(ticket);
            }

            var existingTicket = await _ticketService.GetTicketByIdAsync(ticket.TicketId);

            if (existingTicket == null)
            {
                TempData["Error"] = "Ticket not found.";
                return RedirectToAction(nameof(MyAssignedTickets));
            }

            // Prevent updating a ticket that is already closed
            if (existingTicket.Status == "Closed")
            {
                TempData["Error"] = "This ticket is already closed and cannot be modified.";

                return RedirectToAction(nameof(UpdateStatus), new { id = existingTicket.TicketId });
            }

            existingTicket.Status = ticket.Status;

            await _ticketService.UpdateTicketAsync(existingTicket);

            await _ticketHistoryService.AddHistoryAsync(new TicketHistory
            {
                TicketId = existingTicket.TicketId,
                UserId = _userManager.GetUserId(User),
                Action = $"Status changed to {existingTicket.Status}",
                ActionDate = DateTime.Now
            });

            TempData["Debug"] = "Step 1 Completed";

            try
            {
                var updatedTicket = await _ticketService.GetTicketByIdAsync(existingTicket.TicketId);

                TempData["Debug"] = "Step 2 Completed";

                if (updatedTicket == null)
                {
                    TempData["Error"] = "updatedTicket is NULL";
                    return RedirectToAction(nameof(MyAssignedTickets));
                }

                if (updatedTicket.CreatedBy == null)
                {
                    TempData["Error"] = "CreatedBy is NULL";
                    return RedirectToAction(nameof(MyAssignedTickets));
                }

                if (string.IsNullOrWhiteSpace(updatedTicket.CreatedBy.Email))
                {
                    TempData["Error"] = "Employee Email is NULL";
                    return RedirectToAction(nameof(MyAssignedTickets));
                }

                TempData["Debug"] =
                    $"Email = {updatedTicket.CreatedBy.Email}";

                await _emailService.SendEmailAsync(
                    updatedTicket.CreatedBy.Email,
                    "HelpDesk - Ticket Status Updated",
        $@"Hello {updatedTicket.CreatedBy.FullName},

Your ticket status has been updated.

Ticket Number : {updatedTicket.TicketNumber}
Title         : {updatedTicket.Title}
Status        : {updatedTicket.Status}

Thank you,
HelpDesk Team");

                TempData["Success"] = "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.ToString();
            }

            return RedirectToAction(nameof(MyAssignedTickets));
        }


    }
}