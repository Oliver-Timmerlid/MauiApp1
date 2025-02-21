using Android.Content;
using Android.DeviceLock;
using Android.Telephony;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using Microsoft.Maui.Devices;
using System.Net.WebSockets;

namespace MauiApp1.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly BluetoothAdvertisementService _broadcastService;

    [ObservableProperty]
    private string deviceUuid;

    private Android.Bluetooth.BluetoothAdapter? bluetoothAdapter;
    

    public MainViewModel(BluetoothAdvertisementService broadcastService)
    {
        _broadcastService = broadcastService;
        InitializeBluetoothAdapter();
    }

    private void InitializeBluetoothAdapter()
    {
        bluetoothAdapter = Android.Bluetooth.BluetoothAdapter.DefaultAdapter;
        DeviceUuid = bluetoothAdapter?.Address; // This gives the MAC address, but might be randomized.
    }

    [RelayCommand]
    private void StartBroadcasting()
    {
        //_broadcastService.StartAdvertisementAsync();
    }

    [RelayCommand]
    private void StopBroadcasting()
    {
        //_broadcastService.StopAdvertisement();
    }
}
