using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BDProiect.Models
{
    public class AppContext: DbContext
    {
        public AppContext(): base("DBConnectionString") 
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext, BDProiect.Migrations.Configuration>("DBConnectionString"));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}