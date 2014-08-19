namespace Decko.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    //using Decko.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<Decko.DAL.Mtg2Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Decko.DAL.Mtg2Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var users = new List<User>
            {
                new User {
                    id = 0,
                    username = "root",
                    fname = "Andy",
                    lname = "Ross",
                    dci = "XXXXXXXXX",
                    password = "",
                    email = "andy@awross.me",
                    uid = new Guid().ToString(),
                    admin = true,
                    regIP = "127.0.0.1",
                    updateDate = DateTime.Now,
                    gender = "M"
                }, new User {
                    id = 1,
                    username = "red",
                    fname = "Red",
                    lname = "Test",
                    dci = "XXXXXXXXX",
                    password = "",
                    email = "andy+red@awross.me",
                    uid = new Guid().ToString(),
                    admin = false,
                    regIP = "127.0.0.1",
                    updateDate = DateTime.Now,
                    gender = "M"
                }, new User {
                    id = 1,
                    username = "blue",
                    fname = "Blue",
                    lname = "Test",
                    dci = "XXXXXXXXX",
                    password = "",
                    email = "andy+blue@awross.me",
                    uid = new Guid().ToString(),
                    admin = false,
                    regIP = "127.0.0.1",
                    updateDate = DateTime.Now,
                    gender = "M"
                }
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}
