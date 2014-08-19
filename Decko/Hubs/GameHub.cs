using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Decko.Hubs
{
    public class GameHub : Hub
    {
        public void mulligan(Models.GameCommand data)
        {
            DeckoManager.Game g = Factory.GameManager.Games.Where(x => x.UID == data.gameID).FirstOrDefault();
            g.MulliganPlayer(data.playerID);

            Clients.Group(data.gameID).getState();
        }
    }
}