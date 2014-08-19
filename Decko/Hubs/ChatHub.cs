using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Decko.Hubs
{
    public class ChatHub : Hub
    {
        [Authorize]
        public void Send(string name, string message)
        {
            Clients.All.addNewMsg(name, message);
        }
    }
}