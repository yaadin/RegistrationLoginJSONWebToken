using Microsoft.EntityFrameworkCore;
using LoginWebAPI.Models;

namespace LoginWebAPI.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> users { get; set; }
    }
}
