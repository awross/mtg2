using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckoManager
{
    public class Player
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public PlayerState State { get; set; }

        public int Life { get; set; }
        public int Poison { get; set; }
        public bool Active { get; set; }
        public PlayerDeck Deck { get; set; }
        public PlayerZone Zone { get; set; }

        public Player ()
        {
            Deck = new PlayerDeck();
            Zone = new PlayerZone();

            State = PlayerState.MULLIGAN;
        }
        public void SetUpDeck()
        {
            Zone.SetUpLibrary(UserID, Deck.DeckList.ToList());
            Zone.ShuffleLibrary();
            Zone.ShuffleLibrary();
            Zone.ShuffleLibrary();
            for (int i = 0; i < 7; i++)
            {
                Zone.Draw();
            }
        }

        public void Mulligan()
        {
            Zone.Mulligan();
        }
    }

    public enum PlayerState
    {
        MULLIGAN,
        READY,
        PLAYING,
        SIDEBOARDING,
        FINISHED
    }

    public class PlayerDeck
    {
        public string Name { get; set; }
        public List<int> DeckList { get; set; }
        public List<int> Sideboard { get; set; }

        public PlayerDeck()
        {
            Name = "";
            DeckList = new List<int>();
            Sideboard = new List<int>();
        }
    }

    public class PlayerZone
    {
        private List<CardInstance> Library { get; set; }
        public List<CardInstance> Hand { get; set; }
        public List<CardInstance> Graveyard { get; set; }
        public int LibraryCount
        {
            get
            {
                return Library.Count();
            }
        }

        public void SetUpLibrary(string ownerID, List<int> cards)
        {
            Library = new List<CardInstance>();
            Library = (from c in cards
                       select new CardInstance() {
                           UID = Guid.NewGuid().ToString(),
                           CardID = c,
                           Owner = ownerID,
                           tapped = false
                      }).ToList();
        }

        public void AddLibrary(CardInstance c)
        {
            Library.Add(c);
        }
        public void ShuffleLibrary()
        {
            Random R = new Random(DateTime.Now.Millisecond);
            List<CardInstance> cards = Library.ToList();
            Library.Clear();
            do
            {
                int next = R.Next(cards.Count());
                CardInstance randomCard = cards[next];
                Library.Add(randomCard);
                cards.RemoveAt(next);
            } while (cards.Count() > 0);
        }

        public void Draw()
        {
            Hand.Add(Pop(PlayerZoneType.LIBRARY));
        }

        public void Mulligan()
        {
            int toDraw = Hand.Count() - 1;
            if (toDraw < 0) {toDraw = 0;}
            Library.AddRange(Hand);
            Hand.Clear();
            ShuffleLibrary();
            ShuffleLibrary();
            ShuffleLibrary();
            for (int i=0;i<toDraw;i++)
            {
                Draw();
            }
        }

        public enum PlayerZoneType
        {
            LIBRARY,
            HAND,
            GRAVEYARD
        }
        public CardInstance Pop(PlayerZoneType pzt)
        {
            CardInstance c;
            switch(pzt)
            {
                case PlayerZoneType.LIBRARY:
                    if (Library.Count() > 0) 
                    {
                        c = Library[0];
                        Library.RemoveAt(0);
                        return c;
                    }
                    break;
                case PlayerZoneType.HAND:
                    if (Hand.Count() > 0) 
                    {
                        c = Hand[0];
                        Hand.RemoveAt(0);
                        return c;
                    }
                    break;
                case PlayerZoneType.GRAVEYARD:
                    if (Graveyard.Count() > 0) 
                    {
                        c = Graveyard[0];
                        Graveyard.RemoveAt(0);
                        return c;
                    }
                    break;
                default:
                    Console.WriteLine("Tried to pop unknow player zone");
                    break;
            }

            return null;
        }

        public PlayerZone()
        {
            Library = new List<CardInstance>();
            Hand = new List<CardInstance>();
            Graveyard = new List<CardInstance>();
        }
    }
}
