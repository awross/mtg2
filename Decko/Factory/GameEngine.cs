using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeckoManager;

namespace Decko.Factory
{
    public static class GameManager
    {
        public static List<Game> Games { get; set; }
        public static Lobby Lobby { get; set; }
        public static Random R { get; set; }

        public static List<string> Structures {
            get
            {
                List<string> ret = new List<string>();

                foreach(string name in Enum.GetNames(typeof(StructureType)))
                {
                    ret.Add(name);
                }

                return ret;
            }
        }
        public static List<StructureDrop> GetAllStructures()
        {
            List<StructureDrop> ret = new List<StructureDrop>();

            foreach(string name in Enum.GetNames(typeof(StructureType)))
            {
                ret.Add(new StructureDrop()
                {
                    label = name.ToUpper(),
                    value = name.ToUpper()
                });
            }

            return ret;
        }
        public class StructureDrop
        {
            public string value { get; set; }
            public string label { get; set; }
        }

        static GameManager()
        {
            Games = new List<Game>();
            Lobby = new Lobby();
            R = new Random((int)DateTime.Now.Ticks);
        }


    }

    public class Lobby
    {
        public List<LobbyItem> List { get; set; }
        public Lobby()
        {
            List = new List<LobbyItem>();
            //List.Add(new LobbyItem()
            //{
            //    LobbyID = Guid.NewGuid().ToString(),
            //    Structure = StructureType.ONEVONE,
            //    Status = LobbyStatus.WAITING,
            //    FlagStr = "",
            //    Format = "Modern",
            //    Players = new List<LobbyPlayer>() {
            //        new LobbyPlayer() {
            //            DeckID = "",
            //            DeckName = "Deck 1",
            //            UserID = "",
            //            UserName = "TestUser"
            //        }
            //    },
            //    MaxPlayers = 2
            //});
        }
    }

    public class LobbyItem
    {
        public string LobbyID { get; set; }
        public LobbyStatus Status { get; set; }
        public int MaxPlayers { get; set; }
        public List<LobbyPlayer> Players { get; set; }
        public StructureType Structure { get; set; }
        public string Format { get; set; }
        public string FlagStr { get; set; }

        public LobbyItem()
        {
            Status = LobbyStatus.WAITING;
            Structure = StructureType.OPEN;
        }
    }

    public enum LobbyStatus
    {
        WAITING,
        STARTING,
        PLAYING,
        FINISHED
    }

    public enum StructureType
    {
        ONEVONE,
        OPEN,
        TWOHEADEDGIANT,
        DRAFT
    }

    public class LobbyPlayer
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DeckID { get; set; }
        public string DeckName { get; set; }
    }
}