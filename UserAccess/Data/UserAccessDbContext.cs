using Microsoft.EntityFrameworkCore;
using UserAccessAPI.Models;


namespace UserAccessAPI.Data
{
    public class UserAccessDbContext : DbContext
    {
        //allow you to add some options, like the connection string
        public UserAccessDbContext(DbContextOptions<UserAccessDbContext> options) : base(options) 
        { 
        }

        //DbSet is a representation of the table in the db
        public DbSet<UserDetail> UserDetails { get; set; }
    }
}
