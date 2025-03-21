﻿using Microsoft.Extensions.Logging;
using Shiny;
using Shiny.Notifications;

// Class used for notifications when users have been found
public class Notify
{
    private readonly INotificationManager _notificationManager;
    private readonly ILogger<Notify> _logger;

    public Notify(INotificationManager notificationManager, ILogger<Notify> logger)
    {
        _notificationManager = notificationManager;
        _logger = logger;
    }
    //Send notification
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
                Channel = "SoundChannel",
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
    //Used for soundnotifications
    public void CreateNotificationChannel()
    {
        var existingChannels = _notificationManager.GetChannels();
        
            // Check if the channel "SoundChannel" already exists
        if (existingChannels.Any(c => c.Identifier == "SoundChannel"))
        {
            _logger.LogInformation("Notification channel 'SoundChannel' already exists. Skipping creation.");
            return;
        }
        this._notificationManager.AddChannel(new Channel
        {
            Identifier = "SoundChannel",
            Importance = ChannelImportance.High
        });
    }




}
