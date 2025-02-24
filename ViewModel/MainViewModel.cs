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

namespace MauiApp1.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly BluetoothAdvertisementService _broadcastService;
    private readonly BluetoothAdapter _bluetoothAdapter;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private byte[] encodedName;

    [ObservableProperty]
    private string encodedNameString;

    [ObservableProperty]
    private string uuid;

    public MainViewModel(BluetoothAdvertisementService broadcastService)
    {
        _broadcastService = broadcastService;
        _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

        // Retrieve the Bluetooth adapter MAC address
        string deviceAddress = _bluetoothAdapter?.Address;

        // Generate UUID from Bluetooth adapter MAC address
        Uuid = UUID.NameUUIDFromBytes(Encoding.UTF8.GetBytes(deviceAddress)).ToString();
    }

    [RelayCommand]
    private async Task SaveName()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        //await Shell.Current.GoToAsync("ListPage");

        EncodedName = Encoding.UTF8.GetBytes(Name);
        EncodedNameString = EncodedName != null ? BitConverter.ToString(EncodedName) : "No data";
    }
}
