using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientAddresses;
namespace ClientAddresses
{
    class ClientAddresses:DbContext
    {
        public ClientAddresses():base("DBConnection")
        { }
        public DbSet<Region> Region { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}
