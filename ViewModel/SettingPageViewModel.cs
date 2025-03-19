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
    public partial class SettingPageViewModel : ObservableObject // Define the SettingPageViewModel class, inheriting from ObservableObject
    {
        private readonly FirestoreService _firestoreService; // Firestore service instance
        private readonly string serviceUuid; // UUID for the service
        private User user; // User instance

        [ObservableProperty]
        private string currentName; // Property for the current name

        [ObservableProperty]
        private string changedName; // Property for the changed name

        // Constructor for SettingPageViewModel, accepting a FirestoreService instance
        public SettingPageViewModel(FirestoreService firestoreService)
        {
            _firestoreService = firestoreService; // Initialize the Firestore service
            serviceUuid = GetAndroidIdToUuid(); // Get the UUID for the service
            GetCurrentUser(); // Get the current user
        }
        // Method to get the Android ID and convert it to a UUID
        private string GetAndroidIdToUuid()
        {
            var context = Android.App.Application.Context; // Get the application context
            var androidId = Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId); // Get the Android ID
            return UUID.NameUUIDFromBytes(Encoding.UTF8.GetBytes(androidId)).ToString(); // Convert the Android ID to a UUID
        }
        // Method to get the current user from the Firestore service
        private async void GetCurrentUser()
        {
            User localUser = await _firestoreService.GetUser(serviceUuid); // Get the user from the Firestore service
            if (localUser == null)
            {
                CurrentName = "Inget namn angivet"; // Set the current name to a default value if the user is not found
            }
            else
            {
                CurrentName = localUser.Name; // Set the current name to the user's name
                user = localUser; // Set the user instance
            }
        }
         // Command to handle the save button click
        [RelayCommand]
        private async Task SaveButton()
        {
            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Inget namn", "Gå till startstidan för att sätta namn", "OK"); // Display an alert if the user is not found
            }
            else
            {
                user.Name = ChangedName;
                await _firestoreService.UpdateUser(user);
                GetCurrentUser();
            }
        }
         // Method to load the current user
        public void LoadCurrentUser()
        {
            GetCurrentUser();
        }
    }
}