using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Decko.Models;

namespace Decko.Controllers
{
    /// <summary>
    /// API controller to manage users
    /// </summary>
    public class UsersController : ApiController
    {

        List<user> usersData = new List<user>();
        public IEnumerable<user> Get()
        {
            // Return a static list of users
            return usersData;
        }

        public user Put(user user)
        {
            //Update the user
            return user;
        }

        public user Post(user user)
        {
            return null;
        }
    }
}
