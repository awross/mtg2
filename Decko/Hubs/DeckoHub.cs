using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Decko.Hubs
{
    public class DeckoHub : Hub
    {
        public override Task OnConnected()
        {
            return Clients.Caller.AuthInfo(GetAuthInfo());
        }

        protected object GetAuthInfo()
        {
            var user = Context.User;
            var x = new
            {
                IsAuthenticated = user.Identity.IsAuthenticated,
                IsAdmin = user.IsInRole("Admin"),
                UserName = user.Identity.Name
            };

            return x;
        }
    }
}