﻿using MentorU.Services;
using MentorU.Views;
using Microsoft.Identity.Client;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MentorU
{
    public partial class App : Application
    {

        public static MobileServiceClient client = new MobileServiceClient("https://mentoruapp.azurewebsites.net");

        // MSAL Auth with AAD
        public static IPublicClientApplication AuthenticationClient { get; private set; }
        public static object UIParent { get; set; } = null;

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            //// OLD LOGIN
            //var isLoggedIn = Xamarin.Essentials.SecureStorage.GetAsync("isLogged").Result;
            //if (isLoggedIn == "1")
            //{
            //    MainPage = new AppShell();
            //}
            //else
            //{
            //    MainPage = new LoginPage();
            //}

            AuthenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
                .WithIosKeychainSecurityGroup(Constants.IosKeychainSecurityGroups)
                .WithB2CAuthority(Constants.AuthoritySignin)
                .WithRedirectUri($"msal{Constants.ClientId}://auth")
                .Build();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
