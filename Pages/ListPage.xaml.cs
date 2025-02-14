using MauiApp1.ViewModel;


namespace MauiApp1;

public partial class ListPage : ContentPage
{
	public ListPage(ListPageViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }
    
    
}