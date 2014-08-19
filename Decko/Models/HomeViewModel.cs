using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Decko.Models
{
    public class HomeViewModel
    {
        public RootData root { get; set; }
        public List<DeckFolder> folders { get; set; }

        public string rootJson()
        {
            var ret = new JavaScriptSerializer().Serialize(root);
            return ret;
        }
        public string folderJson()
        {
            var ret = new JavaScriptSerializer().Serialize(folders);
            return ret;
        }

        public HomeViewModel()
        {
            root = new RootData();
        }
    }

    public class RootData
    {
        public string username { get; set; }
        public string userUID { get; set; }
        public List<Navs> navs { get; set; }
        public List<TaskProgress> progress { get; set; }
        public List<Note> notes { get; set; }
        public List<Message> messages { get; set; }

        public RootData()
        {
            navs = new List<Navs>();
            progress = new List<TaskProgress>();
            notes = new List<Note>();
            messages = new List<Message>();
        }
    }

    public class DeckFolder
    {
        public string Name { get; set; }
        public List<DeckLookup> Decks { get; set; }

        public DeckFolder()
        {
            Decks = new List<DeckLookup>();
        }
    }
    
    public class DeckLookup
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    
    public class Navs
    {
        public string name { get; set; }
        public string path { get; set; }
        public string icon { get; set; }
        public List<Navs> SubNavs { get; set; }
    }

    public class TaskProgress
    {
        public string title { get; set; }
        public string type { get; set; }
        public bool striped { get; set; }
        public int percent { get; set; }
    }

    public class Note
    {
        public string icon { get; set; }
        public string color { get; set; }
        public string badgeType { get; set; }
        public string badgeText { get; set; }
        public string text { get; set; }
    }

    public class Message
    {
        public string from { get; set; }
        public string when { get; set; }
        public string msg { get; set; }
        public string avatar { get; set; }
    }
}