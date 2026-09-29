using HelpDesk.DAL.Models;

namespace HelpDesk.DAL.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);

        Task<int> GetUnreadCountAsync(string userId);

        Task AddNotificationAsync(Notification notification);

        Task MarkAsReadAsync(int notificationId, string userId);
    }
}