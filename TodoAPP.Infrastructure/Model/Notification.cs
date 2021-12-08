using TodoAPP.Infrastructure.Enums;

namespace TodoAPP.Infrastructure.Model
{
    public class Notification
    {
        public ENotificationType NotificationType { get; set; }
        public string Message { get; set; }
    }
}