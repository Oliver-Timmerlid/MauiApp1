using Microsoft.Extensions.Logging;
using Shiny;
using Shiny.Notifications;
using System.Security.Permissions;


public class Notify
{
    private readonly INotificationManager _notificationManager;
    private readonly ILogger<Notify> _logger;

    public Notify(INotificationManager notificationManager, ILogger<Notify> logger)
    {
        _notificationManager = notificationManager;
        _logger = logger;
    }

    public async Task SendNotificationAsync(string title, string message)
    {
        var result = await _notificationManager.RequestAccess();
        _logger.LogInformation($"Notification access request result: {result}");

        if (result == AccessState.Available)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                ScheduleDate = null // Set this to schedule it later
            };

            // Schedule the notification
            await _notificationManager.Send(notification);
        }
        else
        {
            _logger.LogWarning("Notification access denied.");
        }
    }
}
