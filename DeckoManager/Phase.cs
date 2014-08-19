using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckoManager
{
    public class PhaseEngine
    {
        private int CurrentPhase { get; set; }
        private List<Phase> Phases { get; set; }
        public PhaseEngine()
        {
            Reset();
        }
        public void Reset()
        {
            CurrentPhase = 0;
            Phases = new List<Phase>()
            {
                new BeginningPhase(),
                new PrecombatMainPhase(),
                new CombatPhase(),
                new PostcombatMainPhase(),
                new EndingPhase()
            };
        }
        public string Current
        {
            get
            {
                return Phases[CurrentPhase].Name;
            }
        }
    }
    public class Phase
    {
        public string Name { get; set; }
        public bool MainStep { get; set; }

        public int CurrentStep { get; set; }
        public List<Step> Steps { get; set; }

        public Phase()
        {
            MainStep = false;

            Steps = new List<Step>();
            CurrentStep = -1;
        }

        public int GetNextStep()
        {
            if (CurrentStep < 0 || CurrentStep == (Steps.Count-1))
            {
                return -1;
            }
            return CurrentStep + 1;
        }

        public int MoveNextStep()
        {
            int nextStep = GetNextStep();
            if (nextStep > 0)
            {
                CurrentStep = nextStep;
                return nextStep;
            }
            return -1;
        }
    }

    public class BeginningPhase : Phase
    {
        public BeginningPhase()
        {
            MainStep = false;
            Name = "Beginning";
            CurrentStep = 0;
            Steps = new List<Step>()
            {
                new Step("untap", false),
                new Step("upkeep"),
                new Step("draw")
            };
        }
        
    }

    public class PrecombatMainPhase : Phase
    {
        public PrecombatMainPhase()
        {
            MainStep = true;
            Name = "Precombat";
            CurrentStep = 0;
            Steps = new List<Step>()
            {
                new Step("precombat")
            };
        }
    }

    public class CombatPhase : Phase
    {
        public CombatPhase()
        {
            MainStep = false;
            Name = "Combat";
            CurrentStep = 0;
            Steps = new List<Step>()
            {
                new Step("beginning"),
                new Step("attackers"),
                new Step("blockers"),
                new Step("damage"),
                new Step("end")
            };
        }
    }
    public class PostcombatMainPhase : Phase
    {
        public PostcombatMainPhase()
        {
            MainStep = true;
            Name = "Postcombat";
            CurrentStep = 0;
            Steps = new List<Step>()
            {
                new Step("postcombat")
            };
        }
    }

    public class EndingPhase : Phase
    {
        public EndingPhase()
        {
            MainStep = false;
            Name = "Ending";
            CurrentStep = 0;
            Steps = new List<Step>()
            {
                new Step("end"),
                new Step("cleanup", false)
            };
        }
    }
}
