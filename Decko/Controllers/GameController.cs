using Decko.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Decko.Controllers
{
    public class GameController : ApiController
    {
        IHubContext GameContext;

        public GameController()
        {
             GameContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
        }

        [HttpPost]
        [Route("game/{id}")]
        public DeckoManager.Game GetGame(string id)
        {
            return Factory.GameManager.Games.Where(x => x.UID == id).FirstOrDefault();
        }

        [HttpPost]
        [Route("game/StartGame/{gameID}/{decision}")]
        public void StartGame(string gameID, string decision)
        {
            DeckoManager.Game game = Factory.GameManager.Games.Where(g => g.UID == gameID).FirstOrDefault();
            string msg = game.PlayerList[game.Active];

            if (decision.ToLower() == "no")
            {
                msg += " decided not to play first";
                game.Active += 1;
                game.Active = game.Active % game.PlayerList.Count();
            }
            else
            {
                msg += " decided to play first";
            }
            if (game.Active > 0)
            {
                game.SetNewFirst();
            }

            game.Msgs.Add(new DeckoManager.GameMsg(msg, DeckoManager.GameMsgType.GAME));
            game.SetUp();

            GameContext.Clients.Group(game.UID).getState();
        }

        [HttpPost]
        [Route("game/GetState/{gameID}/{playerUID}")]
        public Models.BoardViewModel GetState(string gameID, string playerUID)
        {
            DeckoManager.Game g = Factory.GameManager.Games.Where(x => x.UID == gameID).FirstOrDefault();
            DeckoManager.Player p = g.GetPlayer(playerUID);
            Models.BoardViewModel ret = new Models.BoardViewModel()
            {
                Game = g,
                Player = p
            };

            return ret;
        }
    }
}
