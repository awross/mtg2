using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decko.Models.Deck
{
    public class NewDeck
    {
        public string user { get; set; }
        public string parentID { get; set; }
        public string folder { get; set; }
        public string name { get; set; }
        public string format { get; set; }
        public string version { get; set; }
        public List<int> deckList { get; set; }
    }

    public class DeckReturn
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}