using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decko.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string DCI { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Uid { get; set; }
        public bool Admin { get; set; }
        public string RegIP { get; set; }
        public DateTime UpdateDate { get; set; }
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}