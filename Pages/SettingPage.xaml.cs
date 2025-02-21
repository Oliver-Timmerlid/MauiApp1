using MauiApp1.ViewModel;

namespace MauiApp1.Pages;

public partial class SettingPage : ContentPage
{
	public SettingPage(SettingPageViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }
}