using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using Shiny.BluetoothLE.Managed;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Content;
using Android.DeviceLock;
using Android.Telephony;
using Android.Provider;
using Java.Util;
using System.Text;

namespace MauiApp1.ViewModel;

public partial class ListPageViewModel : ObservableObject
{
    private readonly BluetoothScan _bluetoothScan;
    private readonly BluetoothAdvertisementService _bluetoothAdvertisementService;
    private readonly Notify _notify;
    private readonly Guid targetUuid = new("12345678-1234-1234-1234-1234567890ab");
    private string androidId;
    private Guid[] serviceUuids;

    [ObservableProperty]
    private ObservableCollection<ManagedScanResult> _devices;

    [ObservableProperty]
    private bool isScanning;

    [ObservableProperty]
    private bool isToggled;

    

    public ListPageViewModel(BluetoothAdvertisementService advertisementService, BluetoothScan bluetoothScan, Notify notify)
    {
        _bluetoothAdvertisementService = advertisementService;
        _bluetoothScan = bluetoothScan;
        _notify = notify;
        _devices = [];
        androidId = GetAndroidId();
        serviceUuids = new Guid[]
        {
            targetUuid, // filter
            Guid.Parse(androidId) // andoridId
        };
    }

    private string GetAndroidId()
    {
        var context = Android.App.Application.Context;
        var androidId = Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
        return UUID.NameUUIDFromBytes(Encoding.UTF8.GetBytes(androidId)).ToString();
    }

    partial void OnIsToggledChanged(bool value)
    {
        if (value)
        {
            _ = StartScanning();
            _ = _bluetoothAdvertisementService.StartAdvertisementAsync(serviceUuids);
        }
        else
        {
            _bluetoothScan.StopScanning();
            _bluetoothAdvertisementService.StopAdvertisement();
            IsScanning = false;
        }
    }

    private async Task StartScanning()
    {
        IsScanning = true;

        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            IsScanning = false;
            IsToggled = false;
            return;
        }

        // Loop to scan for devices every 10 seconds
        while (IsToggled)
        {
            var scannedDevices = await _bluetoothScan.StartScanningAsync(targetUuid);
            //var scannedDevices = await _bluetoothScan.StartScanningAsync(Guid.Empty);

            _notify.CreateNotificationChannel();

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                foreach (var device in scannedDevices)
                {
                    if (!Devices.Any(d => d.Peripheral.Equals(device.Peripheral)))
                    {
                        Devices.Add(device);

                        _ = _notify.SendNotificationAsync("New Device Found", $"Device: {device.Peripheral.Name}");
                    }
                }
            });

            await Task.Delay(10000); // Pause for 10 seconds
        }

        IsScanning = false;
    }
}
