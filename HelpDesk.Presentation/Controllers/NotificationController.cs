using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Presentation.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var notifications =
                await _notificationService.GetUserNotificationsAsync(userId!);

            return View(notifications);
        }

        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var notifications =
                await _notificationService.GetUserNotificationsAsync(userId);

            var notification = notifications
                .FirstOrDefault(n => n.NotificationId == id);

            if (notification == null)
            {
                return NotFound();
            }

            await _notificationService.MarkAsReadAsync(id, userId);

            // If this notification belongs to a ticket,
            // open that ticket directly.
            if (notification.TicketId.HasValue)
            {
                return RedirectToAction(
                    "Details",
                    "Ticket",
                    new { id = notification.TicketId.Value });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}