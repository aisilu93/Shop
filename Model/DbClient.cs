using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Shop.Model
{
    class DbClient: DbContext
    {
        public DbClient() : base("DefaultConnection")
        {
        }
        public DbSet<good> goods { get; set; }
        public DbSet<user> users { get; set; }
    }
}
