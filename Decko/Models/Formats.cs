using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decko.Models
{
    public static class Formats
    {
        public static List<string> GetAll()
        {
            List<string> ret = new List<string>();

            foreach(string name in Enum.GetNames(typeof(EFormats)))
            {
                ret.Add(name);
            }

            return ret;
        }

        public static List<FormatDrop> GetAllFormats()
        {
            List<FormatDrop> ret = new List<FormatDrop>();

            foreach(string name in Enum.GetNames(typeof(EFormats)))
            {
                ret.Add(new FormatDrop()
                {
                    label = name,
                    value = name
                });
            }

            return ret;
        }

        public class FormatDrop
        {
            public string value { get; set; }
            public string label { get; set; }
        }

        public enum EFormats
        {
            Standard,
            Modern,
            Legacy,
            Vintage,
            Extended,
            Casual
        }
    }
}