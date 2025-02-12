using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using System.Windows.Input;

namespace MauiApp1.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly BluetoothAdvertisementService _broadcastService;

    public MainViewModel(BluetoothAdvertisementService broadcastService)
    {
        _broadcastService = broadcastService;
    }

    [RelayCommand]
    private void  StartBroadcasting()
    {
         _broadcastService.StartAdvertisementAsync();
    }

    [RelayCommand]
    private void StopBroadcasting()
    {
         _broadcastService.StopAdvertisementAsync();
    }
}
