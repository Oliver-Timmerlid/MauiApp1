using System.Windows.Input;
using Microsoft.Maui.Controls;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.Generic;
using Plugin.BLE;
using System.Collections.ObjectModel;

namespace MauiApp1;

public partial class NewPage1 : ContentPage
{
    private IBluetoothLE _bluetoothLE;
    private IAdapter _adapter;
    public List<IDevice> Items { get; set; }

    public NewPage1()
    {
        InitializeComponent();
        BindingContext = this;
        _bluetoothLE = CrossBluetoothLE.Current;
        _adapter = CrossBluetoothLE.Current.Adapter;
        Items = new List<IDevice>();
        DevicesListView.ItemsSource = Items;
    }

    protected override async void OnAppearing()
    {
        BindingContext = this;

        base.OnAppearing();
        await HandleBluetooth();

    }

    private async Task HandleBluetooth()
    {
        if (_bluetoothLE.State == BluetoothState.On)
        {
            var devices = _adapter.GetSystemConnectedOrPairedDevices().ToList();
            if (devices.Count == 0)
            {
                await DisplayAlert("No Device", "No connected or paired devices found.", "OK");
            }
            else
            {
                Items.Clear();
                foreach (var device in devices)
                {
                    Items.Add(device);
                    await DisplayAlert("Device Found", $"Connected to {device.Name}", "OK");
                }
                await DisplayAlert("Devices Found", $"{devices.Count} device(s) found.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Bluetooth Off", "Please turn on Bluetooth.", "OK");
        }
    }

    public ICommand ReturnCommand => new Command(async () => await Shell.Current.GoToAsync(".."));
}
