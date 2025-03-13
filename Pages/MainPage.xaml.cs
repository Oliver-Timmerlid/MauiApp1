using MauiApp1.ViewModel; // Import the ViewModel namespace

namespace MauiApp1; // Define the namespace for the application

public partial class MainPage : ContentPage // Define the MainPage class, inheriting from ContentPage
{
    private MainViewModel ViewModel => BindingContext as MainViewModel; // Property to get the ViewModel from the BindingContext

    // Constructor for MainPage, accepting a MainViewModel instance
    public MainPage(MainViewModel vm)
    {
        InitializeComponent(); // Initialize the components defined in the XAML file
        BindingContext = vm; // Set the BindingContext to the provided ViewModel instance
    }

    // Override the OnAppearing method to perform actions when the page appears
    protected override void OnAppearing()
    {
        base.OnAppearing(); // Call the base class's OnAppearing method
        ViewModel?.GetCurrentUser(); // Call the GetCurrentUser method on the ViewModel, if it is not null
    }
}