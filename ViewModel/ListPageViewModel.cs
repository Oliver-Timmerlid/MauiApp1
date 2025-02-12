using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using Shiny.BluetoothLE.Managed;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiApp1.ViewModel;

public partial class ListPageViewModel : ObservableObject
{
    private readonly BluetoothScan _bluetoothScan;

    [ObservableProperty]
    private ObservableCollection<ManagedScanResult> devices = new();

    [ObservableProperty]
    private bool isLoading;

    public ListPageViewModel(BluetoothScan bluetoothScan)
    {
        _bluetoothScan = bluetoothScan;
    }

    [RelayCommand]
    private async Task ScanForDevicesAsync()
    {
        IsLoading = true;
        Devices.Clear();

        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            IsLoading = false;
            return;
        }

        var scannedDevices = await _bluetoothScan.StartScanningAsync();
        foreach (var device in scannedDevices)
        {
            Devices.Add(device);
        }
        IsLoading = false;
    }
}
