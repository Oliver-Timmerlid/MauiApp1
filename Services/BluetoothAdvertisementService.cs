using Shiny.BluetoothLE.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny.BluetoothLE;
using System.Text;
using System.Security.Cryptography;

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

        public async Task StartAdvertisementAsync(Guid[] serviceUuids) // tar in en array av Guids
        {
            try
            {
                string[] serviceUuidStrings = serviceUuids.Select(uuid => uuid.ToString()).ToArray(); // konverterar Guids till strängar

                await _manager.StartAdvertising(new AdvertisementOptions
                {
                    //ServiceUuids = new string[] { serviceUuid.ToString() }
                    ServiceUuids = serviceUuidStrings


                });
                _logger.LogInformation($"Advertisement started with Service UUIDs: {string.Join(", ", serviceUuidStrings)}");
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
