using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decko.Models
{
    public class CardLookup
    {
        public long id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Set { get; set; }
        public string Type { get; set; }
        public string Cost { get; set; }
    }

    public class DeckItem
    {
        public long id { get; set; }
        public int Qty { get; set; }
        public string color { get; set; }
        public string colorStr
        {
            get
            {
                if (color == "null")
                {
                    return "table-flag-gray";
                }
                else
                {
                    string[] colors = JsonConvert.DeserializeObject<string[]>(color);
                    if (colors.Length > 1)
                    {
                        return "table-flag-gray";
                    }
                    else
                    {
                        string flag = colors[0].ToLower();
                        switch (flag) {
                            case "white":
                                return "table-flag-yellow";
                            default:
                                return "table-flag-" + flag;
                        };
                    }
                }
            }
        }
        public string Set { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Cost { get; set; }
        public string Rarity { get; set; }
    }
}