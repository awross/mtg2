using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckoManager
{
    public class Game
    {
        private GameState State {get; set;}
        private PhaseEngine pEngine {get; set;}
        private List<Player> Players { get; set; }
        private List<MAction> Stack { get; set; }
        private List<CardInstance> Battle {get; set;}
        private List<CardInstance> Exile {get; set;}

        public List<GameMsg> Msgs { get; set; }

        public string UID { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public string Structure { get; set; }
        public int Turn {get; set;}
        public int Active {get; set;}
        public int Priority {get; set;}
        public int Resolve {get; set;}
        public string StateStr
        {
            get
            {
                return State.ToString();
            }
        }

        public string Phase
        {
            get
            {
                return pEngine.Current;
            }
        }
        public string ActivePlayer
        {
            get
            {
                return Players[Active].Name;
            }
        }



        public Game(string _uid, int startingPlayer = 0, string _name = "", string _format = "Casual", string _structure = "OPEN")
        {
            UID = _uid == "" ? Guid.NewGuid().ToString() : _uid;
            Name = _name;
            State = GameState.COINFLIP;

            Turn = 0;
            Active = startingPlayer;
            Priority = 0;
            Resolve = 0;

            pEngine = new PhaseEngine();
            Players = new List<Player>();
            Stack = new List<MAction>();
            Battle = new List<CardInstance>();
            Exile = new List<CardInstance>();
            Msgs = new List<GameMsg>();
        }

        private void ActiveInc()
        {
            Active = (Active + 1) % Players.Count();
        }

        public List<string> PlayerList { 
            get
            {
                return (from p in Players
                        select p.Name).ToList();
            }
        }
        public Player GetPlayer(string uid)
        {
            return (from p in Players
                    where p.UserID == uid
                    select p).FirstOrDefault();
        }

        public void AddPlayer(Player newPlayer)
        {
            Players.Add(newPlayer);
        }

        public void SetNewFirst()
        {
            List<Player> shiftingPlayers = Players.Take(Active).ToList();
            Players.RemoveRange(0, Active);
            Players.AddRange(shiftingPlayers);
        }

        public void MulliganPlayer(string playerID)
        {
            DeckoManager.Player p = Players.Where(x => x.UserID == playerID).FirstOrDefault();
            p.Mulligan();
            ActiveInc();
        }

        public void SetUp()
        {
            foreach (Player p in Players)
            {
                p.SetUpDeck();
            }

            State = GameState.MULLIGAN;
        }
    }

    public class GameMsg
    {
        private GameMsgType TypeE { get; set; }
        public string Type { 
            get
            {
                return TypeE.ToString();
            }
        }
        public string Msg { get; set; }
        public GameMsg(string _msg = "", GameMsgType _type = GameMsgType.GAME)
        {
            TypeE = _type;
            Msg = _msg;
        }
    }

    public enum GameState
    {
        COINFLIP,
        MULLIGAN,
        GAME,
        BOARDING
    }

    public enum GameMsgType
    {
        GAME,
        CHAT
    }
}
