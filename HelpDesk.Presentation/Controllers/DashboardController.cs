using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Models;
using HelpDesk.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Presentation.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ITicketService ticketService,
            UserManager<ApplicationUser> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            DashboardViewModel model = new DashboardViewModel
            {
                TotalTickets = await _ticketService.GetTotalTicketsAsync(),
                OpenTickets = await _ticketService.GetOpenTicketsAsync(),
                InProgressTickets = await _ticketService.GetInProgressTicketsAsync(),
                ResolvedTickets = await _ticketService.GetResolvedTicketsAsync(),
                ClosedTickets = await _ticketService.GetClosedTicketsAsync()
            };

            // User Counts
            model.TotalEmployees =
                (await _userManager.GetUsersInRoleAsync("Employee")).Count;

            model.TotalSupportEngineers =
                (await _userManager.GetUsersInRoleAsync("Support Engineer")).Count;

            model.TotalAdmins =
                (await _userManager.GetUsersInRoleAsync("Admin")).Count;

            // Recent Tickets
            model.RecentTickets = (await _ticketService.GetAllTicketsAsync())
                                    .OrderByDescending(t => t.CreatedDate)
                                    .Take(5)
                                    .ToList();

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                model.UserName = user.FullName;
                model.Email = user.Email ?? "";

                var roles = await _userManager.GetRolesAsync(user);

                model.Role = roles.FirstOrDefault() ?? "";
            }

            return View(model);
        }
    }
}