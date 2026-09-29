using HelpDesk.DAL.Context;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.DAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly HelpDeskDbContext _context;

        public NotificationRepository(HelpDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int notificationId, string userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n =>
                    n.NotificationId == notificationId &&
                    n.UserId == userId);

            if (notification != null)
            {
                notification.IsRead = true;

                await _context.SaveChangesAsync();
            }
        }
    }
}