using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Decko.Models;

namespace Decko.DAL
{
    public class Mtg2Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<Mtg2Context>
    {
        protected override void Seed(Mtg2Context context)
        {
            //base.Seed(context);
        }
    }
}