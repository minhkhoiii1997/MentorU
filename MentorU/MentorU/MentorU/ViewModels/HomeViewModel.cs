﻿using System;
using System.Windows.Input;
using MentorU.Models;
using MentorU.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MentorU.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<User> Mentors { get; }
        public ObservableCollection<MarketplaceItem> MarketItems { get; }
        public Command LoadPageDataCommand { get; }
        public Command<User> MentorTapped { get; }
        public Command<MarketplaceItem> ItemTapped { get; }
        public string UsersName { get => _user.Name; }
        private User _user;

        public HomeViewModel()
        {
            Title = "Home";
            _user = DataStore.GetUser().Result;
            Mentors = new ObservableCollection<User>();
            MarketItems = new ObservableCollection<MarketplaceItem>();
            LoadPageDataCommand = new Command(async () => await ExecuteLoadPageData());
            MentorTapped = new Command<User>(OnMentorSelected);
            ItemTapped = new Command<MarketplaceItem>(OnItemSelected);
        }

        async Task ExecuteLoadPageData()
        {
            IsBusy = true;
            try
            {
                //TODO: pull mentor list data and market place data here

                //REMOVE: once above task has be done
                var mentors = await DataStore.GetMentorsAsync(true);
                foreach(var m in mentors) // TODO: adding all? maybe limit to top three
                {
                    Mentors.Add(m);
                }
                var items = await DataStore.GetItemsAsync(true);
                foreach(var i in items) // TODO: adding all? maybe limit to top three
                {
                    MarketItems.Add(i);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }


        async void OnMentorSelected(User mentor)
        {
            if (mentor == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ViewOnlyProfilePage)}?{nameof(ViewOnlyProfileViewModel.UserID)}={mentor.UserID}");
        }



        async void OnItemSelected(MarketplaceItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }


        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}