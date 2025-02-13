﻿using System;
using System.Threading.Tasks;
using MentorU.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;


namespace MentorU.Services.DatabaseServices
{
    /// <summary>
    /// Manage the connection to the database
    /// </summary>
    public class DatabaseService
    {

        public MobileServiceClient client;
        public ClassList ClassList;

        private  static readonly Lazy<DatabaseService> lazy = new Lazy<DatabaseService>
         (() => new DatabaseService());

        public static DatabaseService Instance { get { return lazy.Value; } }

        private DatabaseService()
        {
            client = new MobileServiceClient("https://mentoruapp.azurewebsites.net");
            ClassList = new ClassList();
        }

        /// <summary>
        /// Check to see if we have record of this user in our database.
        /// Returns TRUE if the user was just created.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> tryCreateAccount(Users user)
        {
            var usersList = await client.GetTable<Users>().Where(u => u.id == user.id).ToListAsync();

            if(usersList.Count > 0) 
            {
                // Add the user here to limit he calls to the data base on login in to one
                // And prevent the user from being overwritten in the DB as it was before
                App.loggedUser = usersList[0]; 
                return false;
            }
            return true;
            
        }

    }
}
