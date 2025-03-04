using CommunityToolkit.Mvvm.ComponentModel;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Content;
using Android.DeviceLock;
using Android.Telephony;
using Android.Provider;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;

namespace MauiApp1.ViewModel
{
    public partial class SettingPageViewModel : ObservableObject
    {
        private readonly FirestoreService _firestoreService;
        private readonly string serviceUuid;
        private User user;

        [ObservableProperty]
        private string currentName;

        [ObservableProperty]
        private string changedName;

        public SettingPageViewModel(FirestoreService firestoreService)
        {
            _firestoreService = firestoreService;
            serviceUuid = GetAndroidIdToUuid();
            GetCurrentUser();
        }
        private string GetAndroidIdToUuid()
        {
            var context = Android.App.Application.Context;
            var androidId = Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
            return UUID.NameUUIDFromBytes(Encoding.UTF8.GetBytes(androidId)).ToString();
        }
        private async void GetCurrentUser()
        {
            User localUser = await _firestoreService.GetUser(serviceUuid);
            if (localUser == null)
            {
                CurrentName = "Inget namn angivet";
            }
            else
            {
                CurrentName = localUser.Name;
                user = localUser;
            }
        }

        [RelayCommand]
        private async Task SaveButton()
        {
            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Inget namn", "Gå till startstidan för att sätta namn", "OK");

            }
            else
            {
                user.Name = ChangedName;
                await _firestoreService.UpdateUser(user);
                //user = await _firestoreService.GetUser(serviceUuid);
                //CurrentName = user.Name;
                GetCurrentUser();
            }

        }
        public void LoadCurrentUser()
        {
            GetCurrentUser();
        }
    }
}
