using MauiApp1.Pages; // Import the Pages namespace

namespace MauiApp1 // Define the namespace for the application
{
    public partial class AppShell : Shell // Define the AppShell class, inheriting from Shell
    {
        public AppShell() // Constructor for AppShell
        {
            InitializeComponent(); // Initialize the components defined in the XAML file
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage)); // Register the route for MainPage
            Routing.RegisterRoute(nameof(ListPage), typeof(ListPage)); // Register the route for ListPage
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage)); // Register the route for SettingPage
        }
    }
}