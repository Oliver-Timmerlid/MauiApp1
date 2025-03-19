using MauiApp1.ViewModel; // Import the ViewModel namespace

namespace MauiApp1.Pages; // Define the namespace for the application

public partial class SettingPage : ContentPage // Define the SettingPage class, inheriting from ContentPage
{
    private SettingPageViewModel ViewModel => BindingContext as SettingPageViewModel; // Property to get the ViewModel from the BindingContext

    // Constructor for SettingPage, accepting a SettingPageViewModel instance
    public SettingPage(SettingPageViewModel vm)
    {
        InitializeComponent(); // Initialize the components defined in the XAML file
        BindingContext = vm; // Set the BindingContext to the provided ViewModel instance
    }

    // Override the OnAppearing method to perform actions when the page appears
    protected override void OnAppearing()
    {
        base.OnAppearing(); // Call the base class's OnAppearing method
        ViewModel?.LoadCurrentUser(); // Call the LoadCurrentUser method on the ViewModel, if it is not null
    }
}