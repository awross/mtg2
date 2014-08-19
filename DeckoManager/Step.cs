using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckoManager
{
    public class Step
    {
        public string Name { get; set; }
        public bool GivesPriority { get; set; }

        public Step(string _name, bool _priority = true)
        {
            Name = _name;
            GivesPriority = _priority;
        }
    }
}
