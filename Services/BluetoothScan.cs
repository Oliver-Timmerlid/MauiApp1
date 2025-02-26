using Shiny.BluetoothLE;
using Shiny.BluetoothLE.Managed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny;
using System.Reactive.Linq;
using System.Collections.ObjectModel;

namespace MauiApp1.Services;

public class BluetoothScan
{
    private readonly IBleManager _bleManager;
    private readonly ILogger<BluetoothScan> _logger;
    private IManagedScan? _scanner;
    private ObservableCollection<ManagedScanResult> _devices;

    public BluetoothScan(IBleManager bleManager, ILogger<BluetoothScan> logger)
    {
        _bleManager = bleManager;
        _logger = logger;
        _devices = new ObservableCollection<ManagedScanResult>();
    }

    public async Task<ObservableCollection<ManagedScanResult>> StartScanningAsync(Guid serviceUuid)
    {
        _logger.LogInformation("Requesting Bluetooth access...");
        var access = await _bleManager.RequestAccess();

        if (access != AccessState.Available)
        {
            _logger.LogWarning("Bluetooth access is not available.");
            return _devices;
        }

        //Tillagt massa skit för filtrering. Kommer returnera 0 devices då vi har ett specifikt UUID
        _logger.LogInformation($"Starting BLE scan for UUID: {serviceUuid}...");
        _scanner = _bleManager.CreateManagedScanner();

        // Clear any existing devices
        _devices.Clear();

        await _scanner.Start(new ScanConfig
        {
            ServiceUuids = new[] { serviceUuid.ToString() } // 👈 Add filter here
        });
        _logger.LogInformation("Scan started successfully.");

        await Task.Delay(TimeSpan.FromSeconds(10)); // Simulate scan duration

        // Subscribe to the Peripherals collection changes
        _scanner.Peripherals
            .ToObservable()
            .Subscribe(result =>
            {
                if (!_devices.Contains(result))
                {
                    _devices.Add(result);
                }
            });

        StopScanning();

        return _devices;
    }

    public void StopScanning()
    {
        _logger.LogInformation("Stopping BLE scan...");
        _scanner?.Stop();
    }
}
