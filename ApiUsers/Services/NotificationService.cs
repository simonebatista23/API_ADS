using System.Collections.Concurrent;

namespace ApiUsers.Services
{
    public class NotificationService
    {
     
        private readonly ConcurrentDictionary<int, List<string>> _notifications = new();

        public void AddNotification(int userId, string message)
        {
            var list = _notifications.GetOrAdd(userId, _ => new List<string>());
            list.Add(message);
            Console.WriteLine($"Notificação adicionada para usuário {userId}: {message}");
        }


        public List<string> GetNotifications(int userId)
        {
            return _notifications.TryGetValue(userId, out var list) ? list.ToList() : new List<string>();
        }

        public void ClearNotifications(int userId)
        {
            _notifications.TryRemove(userId, out _);
        }
    }
}
