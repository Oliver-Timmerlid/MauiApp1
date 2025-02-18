using Shiny.BluetoothLE.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny.BluetoothLE;

namespace MauiApp1.Services
{
    public class BluetoothAdvertisementService
    {
        private readonly IBleHostingManager _manager;
        private readonly ILogger<BluetoothAdvertisementService> _logger;

        public BluetoothAdvertisementService(IBleHostingManager manager, ILogger<BluetoothAdvertisementService> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public async Task StartAdvertisementAsync(Guid serviceUuid)
        {
            try
            {
                await _manager.StartAdvertising(new AdvertisementOptions
                {
                    ServiceUuids = new string[] { serviceUuid.ToString() }
                });
                _logger.LogInformation($"Advertisement started successfully with UUID: {serviceUuid}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start advertisement.");
            }
        }


        public void StopAdvertisement()
        {
            try
            {
                _manager.StopAdvertising();
                _logger.LogInformation("Advertisement stopped successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to stop advertisement.");
            }
        }
    }
}
