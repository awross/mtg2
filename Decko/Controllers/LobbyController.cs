using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Decko.Factory;

namespace Decko.Controllers
{
    public class LobbyController : ApiController
    {
        [HttpPost]
        [Route("lobby/list/{filter}")]
        public List<Models.LobbyListItem> GetList(string filter = "nothing")
        {
            List<LobbyItem> LobbyList = GameManager.Lobby.List.ToList();
            return (from l in GameManager.Lobby.List.ToList()
                    where l.Status == LobbyStatus.WAITING
                    select new Models.LobbyListItem
                    {
                        LobbyID = l.LobbyID,
                        PlayerList = l.Players.Select(i => i.UserName).ToList(),
                        MaxPlayers = l.MaxPlayers,
                        Status = l.Status.ToString(),
                        Structure = l.Structure.ToString().ToUpper(),
                        Format = l.Format,
                        FlagClassStr = l.FlagStr,
                        Locked = false,
                        //Closed = l.Players.Select(i => i.UserName).Contains(User.Identity.Name),
                    }).ToList();

        }
    }
}
