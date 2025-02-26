using Android.Bluetooth;
using Android.Content;
using Android.DeviceLock;
using Android.Telephony;
using Android.Provider;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Util;
using MauiApp1.Services;
using Microsoft.Maui.Devices;
using System.Net.WebSockets;
using System.Text;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace MauiApp1.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly BluetoothAdvertisementService _broadcastService;
    private readonly BluetoothAdapter _bluetoothAdapter;
    private readonly FirestoreService _firestoreService;

    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private string androidId;

    public MainViewModel(BluetoothAdvertisementService broadcastService, FirestoreService firestoreService)
    {
        _broadcastService = broadcastService;
        androidId = GetAndroidId();
        _firestoreService = firestoreService;
    }

    private string GetAndroidId()
    {
        var context = Android.App.Application.Context;
        return Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
    }
    [RelayCommand]
    private async Task SaveName()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }
        User user = new User(Name, AndroidId, UUID.NameUUIDFromBytes(Encoding.UTF8.GetBytes(androidId)).ToString());
        await _firestoreService.InsertUser(user); // copied from movie
    }
}
