using HelpDesk.BAL.Interfaces;
using HelpDesk.DAL.Interfaces;
using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _notificationRepository.GetUserNotificationsAsync(userId);
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _notificationRepository.GetUnreadCountAsync(userId);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task MarkAsReadAsync(int notificationId, string userId)
        {
            await _notificationRepository.MarkAsReadAsync(
                notificationId,
                userId);
        }
    }
}