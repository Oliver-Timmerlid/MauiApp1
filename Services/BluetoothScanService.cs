using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using MauiApp1.Model;

namespace MauiApp1.Services
{
    public class BluetoothScanService
    {
        private readonly IAdapter _adapter;

        public BluetoothScanService(IAdapter adapter)
        {
            _adapter = adapter;
        }

        public async Task<IEnumerable<Model.IDevice>> StartScanningAsync()
        {
            var devices = new List<Model.IDevice>();
            _adapter.DeviceDiscovered += (s, a) => devices.Add(new Model.Device
            {
                Name = a.Device.Name,
                //Id = a.Device.Id.ToString()
            });
            await _adapter.StartScanningForDevicesAsync();
            return devices;
        }

        public async Task StopScanningAsync()
        {
            await _adapter.StopScanningForDevicesAsync();
        }
    }
}
