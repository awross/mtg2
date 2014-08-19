using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Decko.Controllers
{
    public class CardController : ApiController
    {

        Decko.Entities _db;

        public CardController()
        {
            _db = new Entities();
        }

        [Route("cards/name/{term}")]
        public IEnumerable<Models.CardLookup> FindByName(string term)
        {
            List<card> find = (from c in _db.cards
                               orderby c.id descending
                               group c by c.name into g1
                               let MaxID = g1.Max(x => x.id)
                               where g1.Key.Contains(term)
                               select g1.Where(y => y.id == MaxID).FirstOrDefault()).ToList();

            List<Models.CardLookup> ret = (from x in find
                                           select new Models.CardLookup()
                                           {
                                               id = x.id,
                                               Name = x.name,
                                               Set = x.setcode,
                                               Type = x.typeline,
                                               Cost = x.cost,
                                               Active = false
                                           }).ToList();

            return ret;
        }

        [Route("cards/deck/{id}")]
        public Models.DeckItem FindById(int id)
        {
            card find = (from c in _db.cards
                               where c.id == id
                               select c).FirstOrDefault();

            Models.DeckItem newItem = new Models.DeckItem()
            {
                id = find.id,
                color = find.color,
                Set = find.setcode,
                Name = find.name,
                Type = find.typeline,
                Cost = find.cost,
                Rarity = find.rarity,
                Qty = 1
            };

            return newItem;
        }

        [HttpPost]
        [Route("card/id/{id}")]
        public card GetCard(int id)
        {
            card find = (from c in _db.cards
                               where c.id == id
                               select c).FirstOrDefault();

            return find;
        }
    }
}
