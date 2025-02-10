using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using MauiApp1.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiApp1.ViewModel
{
    public partial class ListPageViewModel : ObservableObject
    {
        private readonly BluetoothScanService _scanService;

        [ObservableProperty]
        private ObservableCollection<Model.IDevice> devices = new ObservableCollection<Model.IDevice>();

        public ListPageViewModel(BluetoothScanService scanService)
        {
            _scanService = scanService;
        }

        [RelayCommand]
        private async Task ScanForDevicesAsync()
        {
            Devices.Clear();
            var scannedDevices = await _scanService.StartScanningAsync();
            foreach (var device in scannedDevices)
            {
                Devices.Add(device);
            }
        }
    }
}
