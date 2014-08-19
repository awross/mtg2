using Decko.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Decko.DAL
{
    public class Mtg2Context : DbContext
    {
        public Mtg2Context() : base("mtg2Entities")
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<User>().ToTable("User");
            //modelBuilder.Entity<UserRole>().ToTable("UserRole");
            //modelBuilder.Entity<Set>().ToTable("Set");
            //modelBuilder.Entity<Card>().ToTable("Card");
        }
    }
}