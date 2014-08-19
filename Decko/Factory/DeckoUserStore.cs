using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Decko.Factory
{
    public class DeckoUserStore : IUserStore<DeckoUser>
    {
        private readonly Entities _db;
        public DeckoUserStore(Entities dbContext) { _db = dbContext; }


        public Task CreateAsync(DeckoUser user)
        {
            user u = new user()
            {
                uid = Guid.NewGuid().ToString(),
                username = user.UserName,
                fname = user.Fname,
                lname = user.Lname,
                dci = user.Dci,
                password = user.Password,
                email = user.Email,
                admin = user.Admin,
                gender = user.Gender,
                regIP = user.IpAddress,
                updateDate = DateTime.Now
            };

            _db.users.Add(u);
            _db.Configuration.ValidateOnSaveEnabled = false; // ?? Do I need this??
            return _db.SaveChangesAsync();
        }

        public Task DeleteAsync(DeckoUser user)
        {
            user u = _db.users.Where(m => m.uid.ToString().ToLower() == user.Id).FirstOrDefault();
            _db.users.Remove(u);
            _db.Configuration.ValidateOnSaveEnabled = false;
            return _db.SaveChangesAsync();
        }

        public Task<DeckoUser> FindByIdAsync(string userId)
        {
            return Task.Run<DeckoUser>(() =>
            {
                user u = _db.users.Where(m => m.id.ToString().ToLower() == userId.ToLower()).FirstOrDefault();
                return new DeckoUser()
                {
                    Id = u.uid,
                    UserName = u.username,
                    Fname = u.fname,
                    Lname = u.lname,
                    Dci = u.dci,
                    Password = u.password,
                    Email = u.email,
                    Admin = u.admin,
                    Gender = u.gender
                };
            });
        }

        public Task<DeckoUser> FindByNameAsync(string userName)
        {
            return Task.Run<DeckoUser>(() =>
            {
                user u = _db.users.Where(m => m.username.ToLower() == userName.ToLower()).FirstOrDefault();

                if (u == null)
                {
                    return null;
                }
                else
                {
                    return new DeckoUser()
                    {
                        Id = u.uid,
                        UserName = u.username,
                        Fname = u.fname,
                        Lname = u.lname,
                        Dci = u.dci,
                        Password = u.password,
                        Email = u.email,
                        Admin = u.admin,
                        Gender = u.gender
                    };
                }
            });
        }

        public Task UpdateAsync(DeckoUser user)
        {
            user u = new user()
            {
                uid = Guid.NewGuid().ToString(),
                username = user.UserName,
                fname = user.Fname,
                lname = user.Lname,
                dci = user.Dci,
                password = user.Password,
                email = user.Email,
                admin = user.Admin,
                gender = user.Gender
            };

            _db.users.Attach(u);
            _db.Entry(u).State = EntityState.Modified;
            _db.Configuration.ValidateOnSaveEnabled = false;
            return _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
        /*
        Task IUserStore<DeckoUser, string>.DeleteAsync(DeckoUser user)
        {
            throw new NotImplementedException();
        }

        Task<DeckoUser> IUserStore<DeckoUser, string>.FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        Task<DeckoUser> IUserStore<DeckoUser, string>.FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<DeckoUser, string>.UpdateAsync(DeckoUser user)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
        */
    }
}