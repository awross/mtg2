using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Decko.Models
{
    public class LobbyViewModel
    {
        public List<Models.Formats.FormatDrop> Formats { get; set; }
        public string FormatJson
        {
            get
            {
                return new JavaScriptSerializer().Serialize(Formats);
            }
        }
        public List<Factory.GameManager.StructureDrop> Structures { get; set; }
        public string StructuresJson
        {
            get
            {
                return new JavaScriptSerializer().Serialize(Structures);
            }
        }
    }

    public class LobbyListItem
    {
        public string LobbyID { get; set; }
        //public bool Closed { get; set; }
        public List<string> PlayerList { get; set; }
        public int MaxPlayers { get; set; }
        public string Status { get; set; }
        public string Structure { get; set; }
        public string Format { get; set; }
        public string FlagClassStr { get; set; }
        public bool Locked { get; set; }
    }

    public class NewLobby
    {
        public string UserID { get; set; }
        public int Players { get; set; }
        public string DeckID { get; set; }
        public string DeckName { get; set; }
        public string Format { get; set; }
        public string Structure { get; set; }

        public NewLobby()
        {

        }

        public NewLobby(string _deckID, string _format, int _players, string _structure)
        {
            Players = _players;
            DeckID = _deckID;
            Format = _format;
            Structure = _structure;
        }
    }
    public class JoinLobby
    {
        public string UserID { get; set; }
        public string LobbyID { get; set; }
        public string DeckID { get; set; }
        public string DeckName { get; set; }
    }

}