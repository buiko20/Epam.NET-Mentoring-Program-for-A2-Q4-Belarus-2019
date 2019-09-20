using System.Data.Entity;

namespace Task4
{
    internal class DbContext : System.Data.Entity.DbContext
    {
        public DbContext(string connectionString) : base(connectionString)
        {
            Database.CreateIfNotExists();
        }

        public DbSet<User> Users { get; set; }
    }
}
