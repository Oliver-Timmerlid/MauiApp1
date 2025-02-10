using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly IAdapter _adapter;
        private readonly IBluetoothLE _bluetoothLE;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

        }

        public ICommand StartCommand => new Command(async () =>
        {
            await Navigation.PushAsync(new NewPage1());
        });


    }
}

//private async void StartBtnClicked(object sender, EventArgs e)
//{
//    await Navigation.PushAsync(new NewPage1());
//}


//public ICommand StartCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(NewPage1)));
    
  //  }

//}
