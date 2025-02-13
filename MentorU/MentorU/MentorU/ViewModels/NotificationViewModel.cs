﻿using MentorU.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using MentorU.Views;
using MentorU.Services.DatabaseServices;

namespace MentorU.ViewModels
{
    public class NotificationViewModel : BaseViewModel
    {
        public ObservableCollection<Notification> NotificationList { get; }
        private Notification _notification;

        public Notification Noty { get; set; }

        public Command LoadPageData { get; }
        public Command SelectedCommand { get; }


        public NotificationViewModel()
        {
            NotificationList = new ObservableCollection<Notification>();
            LoadPageData = new Command(async () => await LoadPage());
            SelectedCommand = new Command(async () => await Select());
        }

        async Task LoadPage()
        {
            NotificationList.Clear();
            var notifications = await DatabaseService.Instance.client.GetTable<Notification>().Where(n => n.MentorID == App.loggedUser.id).ToListAsync();
            foreach (Notification n in notifications)
            {
                NotificationList.Add(n);
            }
            if (NotificationList.Count == 0)
            {
                NotificationList.Add(new Notification() { Message = "No Notifications", Seen = true });
            }
            IsBusy = false;
            
        }

        async Task Select()
        {
            try
            {
                if (Noty != null)
                {
                    Noty.Seen = true;
                    Noty.Unseen = false;
                    await DatabaseService.Instance.client.GetTable<Notification>().UpdateAsync(Noty);
                    List<Users> user = await DatabaseService.Instance.client.GetTable<Users>().Where(u => u.id == Noty.MenteeID).ToListAsync();
                    if (user.Count != 0)
                        await App.Current.MainPage.Navigation.PushAsync(new ViewOnlyProfilePage(user[0], false, true));
                }
            }   
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
