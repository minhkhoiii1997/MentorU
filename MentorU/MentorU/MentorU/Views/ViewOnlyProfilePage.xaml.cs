﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MentorU.ViewModels;
using MentorU.Views;
using MentorU.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentorU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewOnlyProfilePage : ContentPage
    {
        ViewOnlyProfileViewModel _vm;
        public ViewOnlyProfilePage(Users u, bool isConnected, bool fromNotification=false)
        {
            InitializeComponent();
            BindingContext = _vm =  new ViewOnlyProfileViewModel(u, isConnected, fromNotification);
            if (isConnected) // Toggles the interactions available to the mentee depending on if they are connected
            {

                //ToolbarItem chatButton = new ToolbarItem
                //{
                //    Text = "Chat",
                //    Command = new Command(_vm.StartChat)
                //};
                ToolbarItem options = new ToolbarItem
                {
                    Text = "Remove",
                    Command = new Command(_vm.OpenOptions)
                };
                //ToolbarItem Schedule = new ToolbarItem
                //{
                //    Text = "Schedule",
                //    Command = new Command(_vm.ScheduleMeeting)
                //};

                ToolbarItems.Add(options);
                //ToolbarItems.Add(chatButton);
                //ToolbarItems.Add(Schedule);
            }
            else if(!fromNotification)
            {
                ToolbarItem requestButton = new ToolbarItem
                {
                    Text = "Request Mentor",
                    Command = new Command(_vm.OnRequestMentor)
                };
                ToolbarItems.Add(requestButton);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.OnAppearing();
        }
    }
}