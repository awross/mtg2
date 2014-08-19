using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Decko.Models.Admin;
using Decko.Models.MtgJson;
using Newtonsoft.Json;
using System.Data.Entity.Validation;

namespace Decko.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        Entities _db;
        List<string> setsAdded = new List<string>();
        public AdminController()
        {
            _db = new Entities();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SetUpload()
        {
            SetUploadViewModel model = new SetUploadViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult SetUpload(Guid id, string UploadType)
        {
            try
            {
                int count = 0;
                setsAdded = new List<string>();
                string msg = "";
                if (HttpContext.Request.Path.Contains("SetUpload"))
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileData = new byte[file.ContentLength];
                        file.InputStream.Read(fileData, 0, file.ContentLength);
                        string result = System.Text.Encoding.UTF8.GetString(fileData);

                        if (UploadType == "list")
                        {
                            Dictionary<string, Models.MtgJson.Set> newSets = JsonConvert.DeserializeObject<Dictionary<string, Models.MtgJson.Set>>(result, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });
                            foreach (KeyValuePair<string, Models.MtgJson.Set> entry in newSets)
                            {
                                ProcessSet(entry.Value);
                                count++;
                            }

                        }
                        else
                        {
                            Models.MtgJson.Set s = JsonConvert.DeserializeObject<Models.MtgJson.Set>(result, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore, TypeNameHandling = TypeNameHandling.Objects });
                            ProcessSet(s);
                            count = 1;
                        }

                    }

                }

                if (count > 0)
                {
                    msg = String.Format(new PluralFormatProvider(), "File Uploaded, {0} new {0:set;sets} added!", count);
                }
                else
                {
                    msg = "No new sets found...";
                }

                return Json(new 
                {
                    message = msg,
                    newSets = setsAdded,
                    success = true 
                });
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
                throw;
            }
            catch (Exception ex)
            {
                return Json(new { message = "ERROR -->  " + ex.Message, success = false });
            }
        }

        private void ProcessSet(Models.MtgJson.Set s)
        {
            bool setCheck = _db.sets.Where(m => m.code == s.code).ToList().Count == 0;
            if (setCheck)
            {
                //Models.MtgJson.Set s = entry.Value;
                Decko.set tempSet = new Decko.set()
                {
                    name = s.name,
                    code = s.code,
                    gathererCode = s.gathererCode,
                    releaseDate = s.releaseDate,
                    border = s.border,
                    type = s.type,
                    //booster = ""
                    booster = JsonConvert.SerializeObject(s.booster)
                };

                _db.sets.Add(tempSet);
                _db.SaveChanges();
                setsAdded.Add(string.Format("{0} {1}", s.code, s.name));

                if (s.cards.Count() > 0)
                {
                    foreach (Models.MtgJson.Card c in s.cards)
                    {
                        Decko.card tempCard = new Decko.card()
                        {
                            multiverseID = c.multiverseid,
                            setcode = s.code,
                            layout = c.layout ?? "",
                            typeline = c.type ?? "",
                            type = JsonConvert.SerializeObject(c.types),
                            subtype = JsonConvert.SerializeObject(c.subtypes),
                            name = c.name ?? "",
                            originalTypeline = c.originalType ?? "",
                            cmc = c.cmc ?? 0,
                            rarity = c.rarity ?? "",
                            artist = c.artist ?? "",
                            color = JsonConvert.SerializeObject(c.colors),
                            cost = c.manaCost ?? "",
                            text = c.text ?? "",
                            originalText = c.originalText ?? "",
                            flavor = c.flavor ?? "",
                            imageName = c.imageName ?? "",
                            power = c.power ?? "",
                            toughness = c.toughness ?? ""
                        };

                        _db.cards.Add(tempCard);
                    }
                    _db.SaveChanges();
                }
            }
        }
    }
}