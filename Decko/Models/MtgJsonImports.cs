using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decko.Models.MtgJson
{
    public class Set
    {
        public string name { get; set; }
        public string code { get; set; }
        public string gathererCode { get; set; }
        public DateTime releaseDate { get; set; }
        public string border { get; set; }
        public string type { get; set; }
        public object[] booster { get; set; }
        public Card[] cards { get; set; }
    }

    public class Card
    {
        public string layout { get; set; }
        public string type { get; set; }
        public string[] types { get; set; }
        public string[] colors { get; set; }
        public int multiverseid { get; set; }
        public string name { get; set; }
        public string[] subtypes { get; set; }
        public string originalType { get; set; }
        public decimal? cmc { get; set; }
        public string rarity { get; set; }
        public string artist { get; set; }
        public string power { get; set; }
        public string toughness { get; set; }
        public string manaCost { get; set; }
        public string text { get; set; }
        public string originalText { get; set; }
        public string flavor { get; set; }
        public string imageName { get; set; }
        //public ForeignName[] foreignNames { get; set; }
        //public Dictionary<string, string> legalities { get; set; }
        //public string[] printings { get; set; }
    }
}

public class ForeignName
{
    public string language { get; set; }
    public string name { get; set; }
}