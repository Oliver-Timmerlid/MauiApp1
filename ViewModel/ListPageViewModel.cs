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
    private readonly FirestoreService _firestoreService;
    private readonly Guid targetUuid = new("12345678-1234-1234-1234-1234567890ab");
    private string androidId;
    //private Guid[] serviceUuids;

    [ObservableProperty]
    private ObservableCollection<ManagedScanResult> _devices;

    [ObservableProperty]
    private ObservableCollection<User> _users;

    [ObservableProperty]
    private bool isScanning;

    [ObservableProperty]
    private bool isToggled;

    

    public ListPageViewModel(BluetoothAdvertisementService advertisementService, BluetoothScan bluetoothScan, Notify notify, FirestoreService firestoreService)
    {
        _bluetoothAdvertisementService = advertisementService;
        _bluetoothScan = bluetoothScan;
        _notify = notify;
        _firestoreService = firestoreService;
        _devices = new ObservableCollection<ManagedScanResult>();
        _users = new ObservableCollection<User>();
        androidId = GetAndroidId();
        //serviceUuids = new Guid[]
        //{
        //    // targetUuid, filter
        //    Guid.Parse(andoidId) // andoridId
        //};
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
            _ = _bluetoothAdvertisementService.StartAdvertisementAsync(androidId);
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
            // eventuell nolla listan
            Users.Clear();
            
            var scannedDevices = await _bluetoothScan.StartScanningAsync();
            _notify.CreateNotificationChannel();

            // eventuell nolla listan

            foreach (var device in scannedDevices)
            {
                
                string serviceUuid = device.ServiceUuids[0].ToString();
                if (!string.IsNullOrEmpty(serviceUuid))
                {
                    var user = await _firestoreService.GetUser(serviceUuid);
                    if (user != null && !Users.Any(u => u.Uuid == user.Uuid))
                    {
                        Users.Add(user);
                        await _notify.SendNotificationAsync($"{user.Name}", " , Vill tala älvdalska");
                    }
                }
                

            }

            await Task.Delay(10000); // Pause for 10 seconds
        }
        IsScanning = false;
    }
}
