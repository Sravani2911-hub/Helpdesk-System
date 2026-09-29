using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);

        Task<int> GetUnreadCountAsync(string userId);

        Task AddNotificationAsync(Notification notification);

        Task MarkAsReadAsync(int notificationId, string userId);
    }
}