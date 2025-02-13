﻿using MentorU.Services.LogOn;
using MentorU.Views;
using MentorU.Views.ChatViews;
using System;
using Xamarin.Forms;

namespace MentorU
{
    public partial class AppShell : Xamarin.Forms.Shell
    {

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(NewJobPage), typeof(NewJobPage));
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(MainChatPage), typeof(MainChatPage));
            Routing.RegisterRoute(nameof(GroupChatMainPage), typeof(GroupChatMainPage));
            Routing.RegisterRoute(nameof(ViewOnlyProfilePage), typeof(ViewOnlyProfilePage));
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(NotificationPage), typeof(NotificationPage));
            Routing.RegisterRoute(nameof(ActivityWaitPage), typeof(ActivityWaitPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ContactUsPage), typeof(ContactUsPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute("Main/Login", typeof(LoginPage));
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            AppShell.Current.FlyoutIsPresented = false;   //close the menu 

            var userContext = await B2CAuthenticationService.Instance.SignOutAsync();
            App.AADUser = userContext;

            //await Shell.Current.Navigation.PopToRootAsync();

            //await GoToAsync("///Login");

            App.Current.MainPage = new AppShell();


        }
    }
}
