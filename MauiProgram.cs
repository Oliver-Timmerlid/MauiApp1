using MauiApp1.Services;
using Microsoft.Extensions.Logging;
using MauiApp1.ViewModel;
using Shiny;
using MauiApp1.Pages;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseShiny()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //
            builder.Services.AddSingleton<FirestoreService>();

            // Register Bluetooth services
            builder.Services.AddBluetoothLeHosting();
            builder.Services.AddBluetoothLE();
            builder.Services.AddLogging();
            builder.Services.AddNotifications();

            //builder.Services.AddSingleton<BluetoothScanService>();
            builder.Services.AddSingleton<BluetoothAdvertisementService>();
            builder.Services.AddSingleton<BluetoothScan>();
            builder.Services.AddSingleton<Notify>();


            // Register pages and view models as singletons
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<ListPage>();
            builder.Services.AddSingleton<ListPageViewModel>();

            builder.Services.AddSingleton<SettingPage>();
            builder.Services.AddSingleton<SettingPageViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
