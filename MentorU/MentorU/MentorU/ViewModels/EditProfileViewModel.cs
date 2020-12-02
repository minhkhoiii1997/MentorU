﻿using MentorU.Models;
using MentorU.Views;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MentorU.ViewModels
{
    public class EditProfileViewModel : BaseViewModel
    {
        private User _user;
        private string _name;
        private string _major;
        private string _bio;
        private ProfileViewModel _vm;
        public string Name 
        { 
            get => _name; 
            set => SetProperty(ref _name, value); 
        }
        public string Major
        { 
            get => _major; 
            set => SetProperty(ref _major, value); 
        }
        public ObservableCollection<string> Classes { get; set; }
        public string Bio
        { 
            get => _bio;
            set => SetProperty(ref _bio, value); 
        }
        public Command SaveButtonCommand { get; set; }
        public Command CancelButtonCommand { get; set; }

        /***
         * Creates a temporary user that saves the changes. If cancelled the _user
         * remains the same. If save the temp values are copied over to the _user
         */
        public EditProfileViewModel(ProfileViewModel profileVM)
        {
            _vm = profileVM;
            _user = DataStore.GetUser().Result;
            _name = _user.Name;
            _major = _user.Major;
            _bio = _user.Bio;
            SaveButtonCommand = new Command(OnSave);
            CancelButtonCommand = new Command(OnCancel);
        }

        private async void OnSave()
        {
            _user.Name = _vm.Name = Name;
            _user.Major = _vm.Major = Major;
            _user.Bio = _vm.Bio = Bio;
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async void OnCancel()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
