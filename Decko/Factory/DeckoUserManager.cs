using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Claims;
using System.Web;
using Decko.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace Decko.Factory
{
    public class DeckoUserManager : UserManager<DeckoUser>
    {
        private readonly Entities _db;
        public DeckoUserManager(DeckoUserStore store)
            : base(store)
        {
            _db = new Entities();
        }

        public override async Task<DeckoUser> FindAsync(string userName, string password)
        {
            //First, check out if password is valid
            //Then, make sure that password matches
            //Last, find user

            DeckoUser user = await Store.FindByNameAsync(userName);
            return user;
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(DeckoUser user, string authenticationType)
        {
            IList<Claim> claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Country, "USA"),
                new Claim(ClaimTypes.Email, user.Email)
            };

            List<userrole> roles = (from u in _db.userroles
                                    where u.username == user.UserName
                                    select u).ToList();

            foreach (userrole r in roles)
            {
                claimCollection.Add(new Claim(ClaimTypes.Role, r.role));
            }

            var claimsIdentity = new ClaimsIdentity(claimCollection, authenticationType);

            return await Task.Run(() => claimsIdentity);  
        }

        public override async Task<IdentityResult> CreateAsync(DeckoUser user, string password)
        {
            user.Password = password;
            //await Store.CreateAsync(user);
            IdentityResult i = await base.CreateAsync(user);
            return i;
        }
    }
}