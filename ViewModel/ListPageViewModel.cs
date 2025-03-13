using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Services;
using Shiny.BluetoothLE.Managed;
using System.Collections.ObjectModel;
using Android.Provider;
using Java.Util;
using System.Text;
using MauiApp1.Models;

namespace MauiApp1.ViewModel;

public partial class ListPageViewModel : ObservableObject
{
    // Dependencies injected via constructor for Bluetooth scanning, notifications, Firestore
    private readonly BluetoothScan _bluetoothScan;
    private readonly BluetoothAdvertisementService _bluetoothAdvertisementService;
    private readonly Notify _notify;
    private readonly FirestoreService _firestoreService;
    private string androidId; // Stores the unique Android ID.

    // Observable properties for data binding in the View
    [ObservableProperty]
    private ObservableCollection<ManagedScanResult> _devices;

    [ObservableProperty]
    private ObservableCollection<User> _users;

    [ObservableProperty]
    private bool isScanning;

    [ObservableProperty]
    private bool isToggled;

    // Constructor initializing services and sets up the Android ID for the device
    public ListPageViewModel(BluetoothAdvertisementService advertisementService, BluetoothScan bluetoothScan, Notify notify, FirestoreService firestoreService)
    {
        _bluetoothAdvertisementService = advertisementService;
        _bluetoothScan = bluetoothScan;
        _notify = notify;
        _firestoreService = firestoreService;
        _devices = new ObservableCollection<ManagedScanResult>();
        _users = new ObservableCollection<User>();
        androidId = GetAndroidId();
    }

    // Helper function to retrieve the Android device unique ID
    private string GetAndroidId()
    {
        var context = Android.App.Application.Context;
        var androidId = Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
        return UUID.NameUUIDFromBytes(Encoding.UTF8.GetBytes(androidId)).ToString();
    }

    // This method is automatically invoked when 'IsToggled' property changes
    partial void OnIsToggledChanged(bool value)
    {
        if (value)
        {
            _ = StartScanning(); // When toggled on, scanning and Bluetooth advertisement starts
            _ = _bluetoothAdvertisementService.StartAdvertisementAsync(androidId);
        }
        else
        {
            _bluetoothScan.StopScanning(); // When toggled off, scanning and Bluetooth advertisement stops
            _bluetoothAdvertisementService.StopAdvertisement();
            IsScanning = false;
        }
    }

    // Method that starts scanning for Bluetooth devices
    private async Task StartScanning()
    {
        User localUser = await _firestoreService.GetUser(androidId);

         // Error message if user hasn't entered their name
        if (localUser == null)
        {
            IsScanning = false;
            IsToggled = false;
            await Application.Current.MainPage.DisplayAlert("Fel", "Du måste ange ett namn innan du kan söka! Gå till Hem och ange namn.", "OK");
            return;
        }
        IsScanning = true;

        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            IsScanning = false;
            IsToggled = false;
            return;
        }

        // Loop to scan for devices every 60 seconds
        while (IsToggled)
        {
            // Clear the user list
            Users.Clear();
            
            var scannedDevices = await _bluetoothScan.StartScanningAsync();
            _notify.CreateNotificationChannel();

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
            await Task.Delay(60000); // Pause for 60 seconds
        }
        IsScanning = false;
    }
}