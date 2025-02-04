using System.Windows.Input;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        //private async void StartBtnClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new NewPage1());
        //}


        public ICommand StartCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(NewPage1)));
    
    }

}
