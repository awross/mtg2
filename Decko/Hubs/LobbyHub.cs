using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Decko.Factory;
using Newtonsoft.Json;

namespace Decko.Hubs
{
    public class LobbyHub : Hub
    {
        Decko.Entities _db;

        public LobbyHub()
        {
            _db = new Entities();
        }
        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }
        public async void AddLobby(Models.NewLobby model)
        {
            StructureType st;
            Decko.Models.Formats.EFormats f;
            if (Enum.IsDefined(typeof(StructureType), model.Structure))
            {
                st = (StructureType)Enum.Parse(typeof(StructureType), model.Structure);
            }
            else { st = StructureType.OPEN; }

            if (Enum.IsDefined(typeof(Decko.Models.Formats.EFormats), model.Format))
            {
                f = (Decko.Models.Formats.EFormats)Enum.Parse(typeof(Decko.Models.Formats.EFormats), model.Format);
            }
            else { f = Decko.Models.Formats.EFormats.Casual; }

            LobbyItem newLobby = new LobbyItem()
            {
                LobbyID = Guid.NewGuid().ToString(),
                Structure = st,
                Format = f.ToString(),
                Players = new List<LobbyPlayer>() {
                    new LobbyPlayer()
                    {
                        DeckID = model.DeckID,
                        DeckName = model.DeckName,
                        UserID = model.UserID,
                        UserName = HttpContext.Current.User.Identity.Name
                    }
                },
                FlagStr = "",
                Status = LobbyStatus.WAITING,
                MaxPlayers = model.Players
            };

            GameManager.Lobby.List.Add(newLobby);

            List<string> pList = (from p in newLobby.Players
                                  select p.UserName).ToList();

            Models.LobbyListItem NewLobbyList = new Models.LobbyListItem()
            {
                LobbyID = newLobby.LobbyID,
                //Closed = false,
                Structure = newLobby.Structure.ToString(),
                Format = newLobby.Format,
                Status = newLobby.Status.ToString().ToUpper(),
                FlagClassStr = "",
                Locked = false,
                PlayerList = pList,
                MaxPlayers = newLobby.MaxPlayers
            };
            Clients.All.addLobby(NewLobbyList);
            await Groups.Add(Context.ConnectionId, newLobby.LobbyID);
        }

        public async void JoinLobby(Models.JoinLobby model)
        {
            bool GameReady = false;
            bool PlayerAdded = false;
            LobbyItem joining;
            lock(GameManager.Lobby)
            {
                joining = GameManager.Lobby.List.Where(m => m.LobbyID == model.LobbyID).FirstOrDefault();
                if (joining.Players.Count() < joining.MaxPlayers)
                {
                    joining.Players.Add(new LobbyPlayer()
                    {
                        DeckID = model.DeckID,
                        DeckName = "JOINING_DECK",
                        UserID = model.UserID,
                        UserName = HttpContext.Current.User.Identity.Name
                    });
                    PlayerAdded = true;
                }
            }
            if (PlayerAdded)
            {
                await Groups.Add(Context.ConnectionId, joining.LobbyID);
                Clients.All.joinLobby(model.LobbyID, HttpContext.Current.User.Identity.Name);
                GameReady = joining.Players.Count() >= joining.MaxPlayers;
                if (GameReady)
                {
                    string newGameName = string.Format("{0}#{1}", joining.Format, joining.LobbyID.Substring(32, 4));
                    DeckoManager.Game newGame = new DeckoManager.Game(
                        joining.LobbyID, 
                        GameManager.R.Next(joining.Players.Count()),
                        newGameName, 
                        joining.Format, 
                        joining.Structure.ToString()
                    );
                    foreach(LobbyPlayer l in joining.Players)
                    {
                        deck playerDeck = _db.decks.Where(x => x.id == l.DeckID).FirstOrDefault();
                        List<int> deckList = JsonConvert.DeserializeObject<List<int>>(playerDeck.decklist);
                        DeckoManager.Player newPlayer = new DeckoManager.Player()
                        {
                            Active = true,
                            Life = 20,
                            Poison = 0,
                            UserID = l.UserID,
                            Name = l.UserName,
                            State = DeckoManager.PlayerState.MULLIGAN,
                            Zone = new DeckoManager.PlayerZone(),
                            Deck = new DeckoManager.PlayerDeck()
                            {
                                Name = playerDeck.name,
                                DeckList = deckList,
                                Sideboard = new List<int>()
                            }
                        };

                        newGame.AddPlayer(newPlayer);
                    }

                    newGame.Msgs.Add(new DeckoManager.GameMsg("Game started, " + newGame.PlayerList[newGame.Active] + " won the coin flip."));
                    lock (GameManager.Games)
                    {
                        GameManager.Games.Add(newGame);
                    }

                    //Clients.All.addGame(newGame);
                    Clients.Group(newGame.UID).addGame(newGame);
                }
            }
        }
    }
}