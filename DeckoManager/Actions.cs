using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckoManager
{
    class MAction
    {
        public string Type { get; set; }
        public string Owner { get; set; }
        public string Controller { get; set; }
        public CardInstance Source { get; set; }
        public List<Target> Targets { get; set; }
    }

    public class Target
    {
        public TargetType Type { get; set; }
        public string TargetID { get; set; }
    }
    public enum TargetType
    {
        PLAYER,
        CARD,
        PERMANANT
    }
}
