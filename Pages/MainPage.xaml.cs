using MauiApp1.ViewModel;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    private MainViewModel ViewModel => BindingContext as MainViewModel;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel?.GetCurrentUser();
    }
}
