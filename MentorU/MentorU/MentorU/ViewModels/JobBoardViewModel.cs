﻿using MentorU.Models;
using MentorU.Services.Blob;
using MentorU.Services.DatabaseServices;
using MentorU.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace MentorU.ViewModels
{
    public class JobBoardViewModel : BaseViewModel
    {
        private Jobs _selectedJob;
        public ObservableCollection<string> AllLevels { get; set; }
        public ObservableCollection<string> AllJobTypes { get; set; }
        private string _level;
        private string _jobType;

        public ObservableCollection<Jobs> Jobs { get; }
        public Command LoadJobsCommand { get; }
        public Command AddJobCommand { get; }
        public Command<Jobs> JobTapped { get; }
        public Command FilterCommand { get; }
        public Command ClearFilters { get; }
        public Command ClosePopUp { get; set; }

        public ObservableCollection<string> Filters { get; }

        private string _filters;
        public string ShowFilters
        {
            get => _filters;
            set
            {
                _filters = value;
                OnPropertyChanged();
            }
        }

        private string _filterJobType;
        public string FilterJobType
        {
            get => _filterJobType;
            set
            {
                _filterJobType = value;
                OnPropertyChanged();
            }
        }

        private string _filterLevel;
        public string FilterLevel
        {
            get => _filterLevel;
            set
            {
                _filterLevel = value;
                OnPropertyChanged();
            }
        }

        public string Level
        {
            get => _level;
            set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        public string JobType
        {
            get => _jobType;
            set
            {
                _jobType = value;
                OnPropertyChanged();
            }
        }

        public JobBoardViewModel()
        {
            Title = "Job Board";
            Jobs = new ObservableCollection<Jobs>();
            Filters = new ObservableCollection<string>();

            // Create a list of levels so that user can select it from this list
            AllLevels = new ObservableCollection<string>()
            {
                "Associate",
                "Entry Level",
                "Mid-Senior level",
                "Executive",
                "Director",
                "Internship"
            };

            // Create a list of job types so that user can select it from this list
            AllJobTypes = new ObservableCollection<string>()
            {
                "Full-time",
                "Part-time",
                "Internship",
                "Contract",
                "Other"
            };

            LoadJobsCommand = new Command(async () => await ExecuteLoadJobsCommand());

            JobTapped = new Command<Jobs>(OnJobSelected);

            AddJobCommand = new Command(OnAddJob);

            FilterCommand = new Command(async () => await ExecuteFilterItems());
            ClearFilters = new Command(async () => { Filters.Clear(); await ExecuteLoadJobsCommand(); });
            ClosePopUp = new Command(async () => await ClosePopUpWindow());
        }

        /// <summary>
        /// Loading the data from database based on filtering
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadJobsCommand()
        {
            IsBusy = true;

            try
            {
                Jobs.Clear();
                if(Filters.Count != 0)
                {
                    var jobs = await DatabaseService.Instance.client.GetTable<Jobs>().ToListAsync();

                    // TODO: will need to implement multiple filters
                    foreach(var job in jobs)
                    {
                        if(Filters.Contains(job.Level) && Filters.Contains(job.JobType))
                        {
                            job.jobImage = await BlobService.Instance.TryDownloadImage("company-logo", job.id);
                            Jobs.Add(job);
                        }
                        else if(Filters.Contains(job.Level) && Filters.Count == 1)
                        {
                            job.jobImage = await BlobService.Instance.TryDownloadImage("company-logo", job.id);
                            Jobs.Add(job);
                        }
                        else if(Filters.Contains(job.JobType) && Filters.Count == 1)
                        {
                            job.jobImage = await BlobService.Instance.TryDownloadImage("company-logo", job.id);
                            Jobs.Add(job);
                        }
                    }

                    ShowFilters = String.Join(", ", Filters);
                }
                else
                {
                    var jobs = await DatabaseService.Instance.client.GetTable<Jobs>().ToListAsync();
                    foreach(var job in jobs)
                    {
                        job.jobImage = await BlobService.Instance.TryDownloadImage("company-logo", job.id);
                        Jobs.Add(job);
                    }
                    ShowFilters = "";
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

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedJob = null;
        }

        public Jobs SelectedJob
        {
            get => _selectedJob;
            set
            {
                SetProperty(ref _selectedJob, value);
                OnJobSelected(value);
            }
        }

        private async void OnAddJob(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewJobPage));
        }

        async void OnJobSelected(Jobs job)
        {
            if (job == null)
                return;

            var application = await DatabaseService.Instance.client.GetTable<Applications>()
                .Where(u => u.ApplicantId == App.loggedUser.id && u.JobId == job.id).ToListAsync();
            bool isApplied = application.Count > 0;
            // This will push the JobDetailPage onto the navigation stack
            await Application.Current.MainPage.Navigation.PushAsync(new JobDetailPage(job, isApplied));
        }

        async Task ExecuteFilterItems()
        {
            await PopupNavigation.Instance.PushAsync(new PopUpJobs(this));
        }

        async Task ClosePopUpWindow()
        {
            if (!string.IsNullOrEmpty(Level))
            {
                Filters.Add(Level);
                Level = "";
            }
            if (!string.IsNullOrEmpty(FilterJobType))
            {
                Filters.Add(FilterJobType);
                FilterJobType = "";
            }
            IsBusy = true;
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
