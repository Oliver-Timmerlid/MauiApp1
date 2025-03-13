using MauiApp1.ViewModel; // Import the ViewModel namespace

namespace MauiApp1; // Define the namespace for the application

public partial class ListPage : ContentPage // Define the ListPage class, inheriting from ContentPage
{
    // Constructor for ListPage, accepting a ListPageViewModel instance
    public ListPage(ListPageViewModel vm)
    {
        InitializeComponent(); // Initialize the components defined in the XAML file
        BindingContext = vm; // Set the BindingContext to the provided ViewModel instance
    }
}