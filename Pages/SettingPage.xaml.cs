using MauiApp1.ViewModel;

namespace MauiApp1.Pages;

public partial class SettingPage : ContentPage
{
    private SettingPageViewModel ViewModel => BindingContext as SettingPageViewModel;
    public SettingPage(SettingPageViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel?.LoadCurrentUser();
    }
}