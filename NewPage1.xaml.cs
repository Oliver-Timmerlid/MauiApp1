using System.Windows.Input;
using Microsoft.Maui.Controls;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.Generic;
using Plugin.BLE;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Shiny.BluetoothLE;
using Shiny.BluetoothLE.Managed;

namespace MauiApp1;

public partial class NewPage1 : ContentPage, INotifyPropertyChanged
{
    private IBluetoothLE _bluetoothLE;
    private IAdapter _adapter;
    private bool _isBusy;
    public List<IDevice> Items { get; set; }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

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
        IsBusy = true;
        if (_bluetoothLE.State == BluetoothState.On)
        {
            Items.Clear(); // Clear the existing list before scanning
            _adapter.DeviceDiscovered += (s, a) =>
            {

                if (!Items.Contains(a.Device)) // Prevent duplicates
                {
                    Items.Add(a.Device);
                }
            };

            await _adapter.StartScanningForDevicesAsync(); // Start scanning

            if (Items.Count == 0)
            {
                await DisplayAlert("No Devices Found", "No nearby Bluetooth devices detected.", "OK");
            }
            else
            {
                //await DisplayAlert("Scan Complete", $"{Items.Count} device(s) found.", "OK");
            }

            DevicesListView.ItemsSource = null;
            DevicesListView.ItemsSource = Items; // Refresh ListView
        }
        else
        {
            await DisplayAlert("Bluetooth Off", "Please turn on Bluetooth.", "OK");
        }
        IsBusy = false;

    }

    public ICommand ReturnCommand => new Command(async () => await Shell.Current.GoToAsync(".."));
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
