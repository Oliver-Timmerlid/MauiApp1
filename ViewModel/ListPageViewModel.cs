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
    private ObservableCollection<ManagedScanResult> _devices;

    [ObservableProperty]
    private bool isLoading;

    public ListPageViewModel(BluetoothScan bluetoothScan)
    {
        _bluetoothScan = bluetoothScan;
        _devices = new ObservableCollection<ManagedScanResult>();
    }

    [RelayCommand]
    private async Task StartScanning()
    {
        IsLoading = true;

        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            IsLoading = false;
            return;
        }

        //Lagt till targetUuid. Detta filtrerar på en specifik tjänst. Kopplas till bluetoothscan.cs
        var targetUuid = new Guid("12345678-1234-1234-1234-1234567890ab");
        var scannedDevices = await _bluetoothScan.StartScanningAsync(targetUuid);
        //var scannedDevices = await _bluetoothScan.StartScanningAsync(Guid.Empty);
        Devices = new ObservableCollection<ManagedScanResult>(scannedDevices);
     
        IsLoading = false;
    }
}
