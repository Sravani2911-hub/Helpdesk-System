using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Presentation.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationViewComponent(
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            if (string.IsNullOrEmpty(userId))
            {
                return View(new List<Notification>());
            }

            var notifications =
                await _notificationService.GetUserNotificationsAsync(userId);

            ViewBag.UnreadCount =
                await _notificationService.GetUnreadCountAsync(userId);

            return View(notifications.Take(5));
        }
    }
}