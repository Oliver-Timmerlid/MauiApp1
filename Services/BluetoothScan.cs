using Shiny.BluetoothLE;
using Shiny.BluetoothLE.Managed;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny;
using System.Reactive.Linq;

namespace MauiApp1.Services;

public class BluetoothScan
{
    private readonly IBleManager _bleManager;
    private readonly ILogger<BluetoothScan> _logger;
    private IManagedScan? _scanner;
    private List<ManagedScanResult> _devices = new();

    public BluetoothScan(IBleManager bleManager, ILogger<BluetoothScan> logger)
    {
        _bleManager = bleManager;
        _logger = logger;
    }

    public async Task<IEnumerable<ManagedScanResult>> StartScanningAsync()
    {
        _logger.LogInformation("Requesting Bluetooth access...");
        var access = await _bleManager.RequestAccess();

        if (access != AccessState.Available)
        {
            _logger.LogWarning("Bluetooth access is not available.");
            return _devices;
        }

        _logger.LogInformation("Starting BLE scan...");
        _devices.Clear(); // Clear previous results

        _scanner = _bleManager.CreateManagedScanner();
        if (_scanner == null)
        {
            _logger.LogError("Failed to create ManagedScanner.");
            return _devices;
        }

        // Attach event handler
        _scanner.Peripherals.CollectionChanged += OnPeripheralDiscovered;

        try
        {
            await _scanner.Start();
            _logger.LogInformation("Scan started successfully.");

            // Scan for longer (30 seconds)
            await Task.Delay(TimeSpan.FromSeconds(30));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while scanning for Bluetooth devices.");
        }
        finally
        {
            StopScanning();
        }

        return _devices;
    }

    private void OnPeripheralDiscovered(object? sender, NotifyCollectionChangedEventArgs e)
    {
        _logger.LogInformation("OnPeripheralDiscovered event triggered.");

        if (_scanner == null)
        {
            _logger.LogWarning("Scanner is null in OnPeripheralDiscovered.");
            return;
        }

        if (_scanner.Peripherals.Count == 0)
        {
            _logger.LogWarning("No peripherals found in OnPeripheralDiscovered.");
        }

        foreach (var peripheral in _scanner.Peripherals)
        {
            // More logging for debugging
            _logger.LogInformation($"Checking Device: {peripheral.Peripheral?.Name} | UUID: {peripheral.Uuid}");

            if (!_devices.Any(d => d.Peripheral.Equals(peripheral.Peripheral)))
            {
                _logger.LogInformation($"Discovered Device: Name={peripheral.Peripheral?.Name}, RSSI={peripheral.Rssi}, UUID={peripheral.Uuid}");
                _devices.Add(peripheral);
            }
        }

        if (_devices.Count > 0)
        {
            _logger.LogInformation($"Total Devices Found: {_devices.Count}");
        }
    }

    public void StopScanning()
    {
        if (_scanner == null)
        {
            _logger.LogWarning("Attempted to stop scan, but scanner is null.");
            return;
        }

        _logger.LogInformation("Stopping BLE scan...");
        _scanner.Stop();

        // Unsubscribe from event to prevent memory leaks
        _scanner.Peripherals.CollectionChanged -= OnPeripheralDiscovered;
    }
}
