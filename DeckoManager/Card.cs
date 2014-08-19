using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckoManager
{
    public class Card
    {
        public int Id { get; set; }
        public int Number {get; set; }
        public string Name {get; set; }
        public int SetNo {get; set; }
        public string Rarity {get; set; }
        public bool Legendary {get; set; }
        public string Type {get; set; }
        public string Subtype {get; set; }
        public string Power {get; set; }
        public string Toughness {get; set; }
        public string Cost {get; set; }
        public int CMC {get; set; }
        public string Text {get; set; }
        public string Flavor {get; set; }
        public string Illus {get; set; }
        public string Abrev {get; set; }
    }

    public class CardInstance
    {
        public string UID { get; set; }
        public int CardID { get; set; }
        public string Owner { get; set; }
        public bool tapped { get; set; }
    }
}
