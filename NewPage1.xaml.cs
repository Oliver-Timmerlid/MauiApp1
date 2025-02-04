using System.Windows.Input;

namespace MauiApp1;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();
        BindingContext = this;
    }
    //private async void ReturnBtnClicked(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new MainPage());
    //}


    public ICommand ReturnCommand => new Command(async () => await Shell.Current.GoToAsync(".."));
}