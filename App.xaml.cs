namespace MauiApp1 // Define the namespace for the application
{
    public partial class App : Application // Define the App class, inheriting from Application
    {
        public App() // Constructor for App
        {
            InitializeComponent(); // Initialize the components defined in the XAML file

            MainPage = new AppShell(); // Set the MainPage to an instance of AppShell
        }
    }
}