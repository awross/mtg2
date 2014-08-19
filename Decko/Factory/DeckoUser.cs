using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decko.Factory
{
    public class DeckoUser : IUser
    {
        private string _id;
        private string _user;
        private string _pass;
        private string _fname;
        private string _lname;
        private string _dci;
        private string _email;
        private bool _admin;
        private string _gender;
        private string _ipAddress;

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string UserName
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        public string Password
        {
            get
            {
                return _pass;
            }
            set
            {
                _pass = value;
            }
        }

        public string Fname
        {
            get
            {
                return _fname;
            }
            set
            {
                _fname = value;
            }
        }

        public string Lname
        {
            get
            {
                return _lname;
            }
            set
            {
                _lname = value;
            }
        }

        public string FullName
        {
            get
            {
                return Fname + " " + Lname;
            }
        }

        public string Dci
        {
            get
            {
                return _dci;
            }
            set
            {
                _dci = value;
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        public bool Admin
        {
            get
            {
                return _admin;
            }
            set
            {
                _admin = value;
            }
        }

        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
            }
        }
        public string IpAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
            }
        }


        string IUser<string>.Id
        {
            get { return _id; }
        }

        string IUser<string>.UserName
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }
    }
}