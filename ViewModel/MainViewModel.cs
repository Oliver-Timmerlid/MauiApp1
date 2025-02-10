using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using System.Windows.Input;

namespace MauiApp1.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly BluetoothBroadcastService _broadcastService;

    public MainViewModel(BluetoothBroadcastService broadcastService)
    {
        _broadcastService = broadcastService;
    }

    [RelayCommand]
    private async Task StartBroadcasting()
    {
        await _broadcastService.StartBroadcastingAsync();
    }

    [RelayCommand]
    private async Task StopBroadcasting()
    {
        await _broadcastService.StopBroadcastingAsync();
    }
}
