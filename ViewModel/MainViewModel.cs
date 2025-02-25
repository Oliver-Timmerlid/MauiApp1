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

    [ObservableProperty]
    private string name;

    private readonly string _androidId;

    public MainViewModel(BluetoothAdvertisementService broadcastService)
    {
        _broadcastService = broadcastService;
        _androidId = GetAndroidId();
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

    }
}
