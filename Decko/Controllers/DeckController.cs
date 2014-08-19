using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Decko.Controllers
{
    public class DeckController : ApiController
    {
        Entities _db;

        public DeckController()
        {
            _db = new Entities();
        }

        [Route("deck/folder/new/{user}/{name}")]
        public string NewFolder(string user, string name)
        {
            deckfolder Folder = new deckfolder()
            {
                id = Guid.NewGuid().ToString(),
                userID = user,
                FolderName = name
            };

            _db.deckfolders.Add(Folder);
            _db.SaveChanges();

            return Folder.FolderName;
        }

        [HttpPost]
        [Route("deck/load/{id}")]
        public deck Load(string id)
        {
            return (from d in _db.decks
                    where d.id == id
                    select d).FirstOrDefault();
        }


        [HttpPost]
        public deck Save(Models.Deck.NewDeck model)
        {
            string newID = "";
            string parent = "";
            if (!string.IsNullOrEmpty(model.parentID))
            {
                char[] arr = model.parentID.Where(c => (char.IsLetterOrDigit(c) || c == '-')).ToArray();
                parent = new string(arr);
                newID = Guid.NewGuid().ToString();
            }
            else
            {
                parent = Guid.NewGuid().ToString();
                newID = parent;
            }

            deck newDeck = new deck
            {
                id = newID,
                parentID = parent,
                userID = model.user,
                folderName = model.folder,
                format = model.format,
                name = model.name,
                decklist = new JavaScriptSerializer().Serialize(model.deckList),
                sideboard = "",
                version = model.version,
                postDate = DateTime.Now
            };

            try
            {
                _db.decks.Add(newDeck);
                _db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }


            return newDeck;
        }
    }
}
