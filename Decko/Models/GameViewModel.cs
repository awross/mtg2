using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decko.Models
{
    public class GameViewModel
    {
        public DeckoManager.Game Game { get; set; }

        public GameViewModel(string id)
        {
            Game = Factory.GameManager.Games.Where(m => m.UID == id).FirstOrDefault();
        }
    }

    public class GameCommand
    {
        public string gameID { get; set; }
        public string playerID { get; set; }
    }

    public class BoardViewModel
    {
        public DeckoManager.Game Game { get; set; }
        public DeckoManager.Player Player { get; set; }
    }
}