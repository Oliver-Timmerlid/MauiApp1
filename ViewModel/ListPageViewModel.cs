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
using MauiApp1.Models;

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
        

        User localUser = await _firestoreService.GetUser(androidId);

        //felmeddelande utan namn
        if (localUser == null)
        {
            IsScanning = false;
            IsToggled = false;
            await Application.Current.MainPage.DisplayAlert("Fel", "Du måste ange ett namn innan du kan söka! Gå till Hem och ange namn.", "OK");
            return;
        }
        IsScanning = true;

        //var intent = new Intent(Android.Bluetooth.BluetoothAdapter.ActionRequestEnable);
        //Platform.CurrentActivity.StartActivity(intent);
        // Loop to scan for devices every 10 seconds
        await EnableBluetoothAndLocationAsync();
        while (IsToggled)
        {
            // eventuell nolla listan
            Users.Clear();
            
            var scannedDevices = await _bluetoothScan.StartScanningAsync();
            _notify.CreateNotificationChannel();

            // eventuell nolla listan


            foreach (var device in scannedDevices)
            {
                if (device.ServiceUuids != null)
                {
                    string serviceUuid = device.ServiceUuids[0].ToString();
                    if (!string.IsNullOrEmpty(serviceUuid))
                    {
                        var user = await _firestoreService.GetUser(serviceUuid);
                        if (user != null && !Users.Any(u => u.Uuid == user.Uuid))
                        {
                            Users.Add(user);

                            await _notify.SendNotificationAsync($"{user.Name}", " Vill tala älvdalska");
                        }
                    }
                }
                
            }

            await Task.Delay(15000); // Pause for 10 seconds
        }
        IsScanning = false;
    }

    public async Task EnableBluetoothAndLocationAsync()
    {
        if (!IsBluetoothEnabled())
        {
            await RequestBluetoothEnableAsync();  // Wait for Bluetooth to be enabled
        }

        if (!IsLocationEnabled())
        {
            OpenLocationSettings();  // Then open Location settings
        }
    }

    public Task RequestBluetoothEnableAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        var intent = new Intent(BluetoothAdapter.ActionRequestEnable);
        Platform.CurrentActivity.StartActivity(intent);

        Task.Run(async () =>
        {
            await Task.Delay(3000); // Wait for 3 seconds before checking again
            tcs.TrySetResult(IsBluetoothEnabled());
        });

        return tcs.Task;
    }

    public static bool IsBluetoothEnabled()
    {
        var bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        return bluetoothAdapter != null && bluetoothAdapter.IsEnabled;
    }

    public static bool IsLocationEnabled()
    {
        var locationManager = (Android.Locations.LocationManager)Platform.CurrentActivity.GetSystemService(Android.Content.Context.LocationService);
        return locationManager.IsProviderEnabled(Android.Locations.LocationManager.GpsProvider) ||
               locationManager.IsProviderEnabled(Android.Locations.LocationManager.NetworkProvider);
    }

    public void OpenLocationSettings()
    {
        var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
        Platform.CurrentActivity.StartActivity(intent);
    }

}
