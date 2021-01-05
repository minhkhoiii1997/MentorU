﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace SignalRServer.Hubs
{
    public class MessageHub : Hub
    {
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
            //await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
            //await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMessage", message);
        }

    }
}
