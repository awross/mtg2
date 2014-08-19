using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Decko.Models;
using Decko.Factory;
using Decko.Hubs;

namespace Decko.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        Entities _db;

        public HomeController()
        {
            _db = new Decko.Entities();
        }

        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();

            user dbUser = _db.users.Where(m => m.username == User.Identity.Name).FirstOrDefault();

            List<DeckoManager.Game> games = (from g in GameManager.Games
                                             where g.PlayerList.Contains(User.Identity.Name)
                                             select g).ToList();
            List<Navs> GameNavs = (from x in games
                                   select new Navs()
                                   {
                                       icon = "",
                                       name = x.Name,
                                       path = "#/game/" + x.UID
                                   }).ToList();

            model.root = new RootData()
            {
                username = HttpContext.User.Identity.Name,
                userUID = dbUser.uid,
                navs = new List<Navs>()
                {
                    new Navs()
                    {
                        name = "Dashboard",
                        path = "#/home",
                        icon = "fa fa-dashboard",
                        SubNavs = new List<Navs>()
                    },
                    //new Navs()
                    //{
                    //    name = "Collection",
                    //    path = "#/collection",
                    //    icon = "fa fa-book"
                    //},
                    new Navs()
                    {
                        name = "Deck Builder",
                        path = "#/build",
                        icon = "fa fa-wrench",
                        SubNavs = new List<Navs>()
                    },
                    //new Navs()
                    //{
                    //    name = "Solo",
                    //    path = "#/solo",
                    //    icon = "fa fa-flask"
                    //},
                    new Navs()
                    {
                        name = "Lobby",
                        path = "#/lobby",
                        icon = "glyphicon glyphicon-tower",
                        SubNavs = GameNavs
                    },
                    new Navs()
                    {
                        name = "Settings",
                        path = "#/settings",
                        icon = "fa fa-cogs",
                        SubNavs = new List<Navs>()
                    },
                    new Navs()
                    {
                        name = "Chat",
                        path = "#/chat",
                        icon = "fa fa-envelope",
                        SubNavs = new List<Navs>()
                    }
                },
                progress = new List<TaskProgress>()
                {
                    new TaskProgress()
                    {
                        title = "Software Update",
                        type = "warning",
                        striped = false,
                        percent = 75
                    },
                    new TaskProgress()
                    {
                        title = "Transfer To New Server",
                        type = "danger",
                        striped = false,
                        percent = 45
                    },
                    new TaskProgress()
                    {
                        title = "Bug Fixes",
                        type = "",
                        striped = false,
                        percent = 75
                    },
                    new TaskProgress()
                    {
                        title = "Writing Documentation",
                        type = "success",
                        striped = false,
                        percent = 85
                    }
                },
                notes = new List<Note>()
                {
                    new Note()
                    {
                        icon = "fa fa-comment",
                        color = "orange",
                        badgeType = "badge badge-warning",
                        badgeText = "4",
                        text = "New Comments"
                    },
                    new Note()
                    {
                        icon = "fa fa-twitter",
                        color = "orange",
                        badgeType = "badge badge-info",
                        badgeText = "7",
                        text = "New Twitter Followers"
                    },
                    new Note()
                    {
                        icon = "fa fa-bug",
                        color = "pink",
                        badgeType = "",
                        badgeText = "",
                        text = "New bug in program!"
                    },
                    new Note()
                    {
                        icon = "fa fa-shopping-cart",
                        color = "green",
                        badgeType = "badge badge-success",
                        badgeText = "+11",
                        text = "You have some new orders"
                    }
                },
                messages = new List<Message>()
                {
                    new Message()
                    {
                        from = "Sarah",
                        when = "a moment ago",
                        msg = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
                        avatar = "avatar3.jpg"
                    },
                    new Message()
                    {
                        from = "Emma",
                        when = "2 Days ago",
                        msg = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris",
                        avatar = "avatar4.jpg"
                    },
                    new Message()
                    {
                        from = "John",
                        when = "8:24 PM",
                        msg = "Duis aute irure dolor in reprehenderit in",
                        avatar = "avatar5.jpg"
                    }
                }
            };

            if (User.IsInRole("Admin"))
            {
                Navs adminNav = new Navs()
                {
                    name = "Admin",
                    path = "#/admin",
                    icon = "fa fa-group"
                };
                model.root.navs.Insert(1, adminNav);
            }

            model.folders = (from f in _db.deckfolders
                             where f.userID == dbUser.uid
                             select new Models.DeckFolder
                             {
                                 Name = f.FolderName
                             }).ToList();

            List<deck> Decks = (from d in _db.decks
                                    orderby d.version descending
                                    where d.userID == dbUser.uid
                                    group d by d.parentID into g1
                                    let MaxVersion = g1.Max(x => x.version)
                                    select g1.Where(y => y.version == MaxVersion).FirstOrDefault()).ToList();

            foreach (Models.DeckFolder f in model.folders)
            {
                f.Decks = (from d in Decks
                           where d.folderName == f.Name
                           orderby d.postDate descending
                           select new DeckLookup
                           {
                               ID = d.id,
                               Name = d.name
                           }).ToList();
            }

            return View(model);
        }
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Build()
        {
            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Lobby()
        {
            LobbyViewModel model = new LobbyViewModel();
            model.Formats = Decko.Models.Formats.GetAllFormats();
            model.Structures = Factory.GameManager.GetAllStructures();

            return View(model);
        }
        public ActionResult Game()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }
    }
}
